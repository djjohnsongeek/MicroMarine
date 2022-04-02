using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Zand;
using Zand.ECS.Components;

namespace MicroMarine.Components
{
    public class UnitGroupManager : SceneComponent
    {
        private List<UnitGroup> UnitGroups;
        private HashSet<string> GroupIds;

        public UnitGroupManager(Scene scene) : base(scene)
        {
            // TODO implement unitgroup pool?
            UnitGroups = new List<UnitGroup>(10);
            GroupIds = new HashSet<string>(10);
        }

        public override void Update()
        {
            if (Input.RightMouseWasPressed())
            {
                CreateOrAssignUnitGroup();
            }

            UpdateUnitGroups();
        }

        private void UpdateUnitGroups()
        {
            for (int i = UnitGroups.Count - 1; i >= 0; i--)
            {
                // Cull empty or stale unit groups
                if (UnitGroups[i].CurrentState == UnitGroupState.Idle || UnitGroups[i].Units.Count == 0)
                {
                    GroupIds.Remove(UnitGroups[i].Id);
                    UnitGroups.RemoveAt(i);
                    continue;
                }

                UnitGroups[i].Update();
            }
        }

        private void CreateOrAssignUnitGroup()
        {
            // TODO reuse a unit group if it's exactly the same? (just need to update the destination)
            List<Entity> units = Scene.GetComponent<UnitSelector>().GetSelectedUnits();
            Vector2 destination = Scene.Camera.GetWorldLocation(Input.MouseScreenPosition);
            string groupId = GetGroupId(units);

            if (GroupIds.Contains(groupId))
            {
                ReuseUnitGroup(groupId, destination);
            }
            else
            {
                RegisterNewGroup(groupId, new UnitGroup(units, destination));
            }
        }

        private void RegisterNewGroup(string groupId, UnitGroup group)
        {
            group.Id = groupId;
            StealUnits(group);
            UnitGroups.Add(group);
            GroupIds.Add(group.Id);

            // really only for debug
            group._scene = Scene;
        }
        private void ReuseUnitGroup(string groupId, Vector2 destination)
        {
            UnitGroup group = GetUnitGroupById(groupId);
            if (Input.RightShiftClickOccured())
            {
                group.Waypoints.Enqueue(destination);
            }
            else
            {
                group.Waypoints.Clear();
                group.CurrentWaypoint = destination;
            }
        }

        private void StealUnits(UnitGroup group)
        {
            // a rather nieve implementation
            for (int i = 0; i < group.Units.Count; i ++)
            {
                for (int j = 0; j < UnitGroups.Count; j++)
                {
                    if (UnitGroups[j].Units.Contains(group.Units[i]))
                    {
                        UnitGroups[j].Units.Remove(group.Units[i]);
                        UnitGroups[j].AssignNewLeader();
                        break;
                    }
                }
            }
        }

        private string GetGroupId(List<Entity> entities)
        {
            // hash with prime numbrtd
            // or bit map
            entities.Sort(CompareEntites);
            var builder = new System.Text.StringBuilder();
            for (int i = 0; i < entities.Count; i ++)
            {
                builder.Append(entities[i].Id);
            }

            return builder.ToString();
        }

        private UnitGroup GetUnitGroupById(string id)
        {
            for (int i = 0; i < UnitGroups.Count; i++)
            {
                if (UnitGroups[i].Id == id)
                {
                    return UnitGroups[i];
                }
            }

            return null;
        }

        private static int CompareEntites(Entity x, Entity y)
        {
            if (x.Id == y.Id)
            {
                return 0;
            }

            if (x.Id > y.Id)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }

    public enum UnitGroupState
    {
        Moving,
        Arrived,
        Grouping,
        Idle,
    }

    public class UnitGroup
    {
        // JUST FOR DEBUG
        public Scene _scene;


        public UnitGroupState CurrentState;
        public string Id;
        public List<Entity> Units;
        public Entity Leader = null;
        public Queue<Vector2> Waypoints;
        public Vector2? CurrentWaypoint;

        private int _followLeaderDist = 0;
        private static int _followLeaderBase = 255;
        private static float _matchFactor = 0.125f;
        private static float _cohesionFactor = .5f;
        private static float _avoidFactor = .8f;
        private static int _maxDistanceSqrd = 25 * 25;
        private static float _unitSpeed = .1F;
        private static float _arrivalThreshold = 1;
        private static float _destinationFactor = 100F;
        private static float _cohesionVelocityLimit = 20F;
        private static int _groupingFrameLimit = 10;
        private static float _circlePackingConst = 1.1026577908435840990226529966259F;

        private float StopDistance = 0;

        public UnitGroup(List<Entity> units, Vector2 destination)
        {
            // TODO: use a pool?
            Waypoints = new Queue<Vector2>();
            Waypoints.Enqueue(destination);
            CurrentWaypoint = null;
            InitGroup(units);
        }

        private void InitGroup(List<Entity> units)
        {
            // Init properties
            Units = units;
            Leader = null;
            AssignNewLeader();
            if (GetNextWaypoint())
            {
                CurrentState = UnitGroupState.Moving;
            }
        }

        public void AssignNewLeader()
        {
            Vector2 centerOfMass = GetCenterOfMass();
            float distance = float.MaxValue;

            for (int i = 0; i < Units.Count; i++)
            {
                float unitDistance = Vector2.Distance(Units[i].Position, centerOfMass);
                if (unitDistance < distance)
                {
                    Leader = Units[i];
                    distance = unitDistance;
                }

                Units[i].GetComponent<UnitState>().CurrentState = UnitStates.Running;
            }

            SetFollowLeaderDistance();
        }

        public void Update()
        {
            switch(CurrentState)
            {
                case UnitGroupState.Moving:
                    Vector2 centerOfMass = GetCenterOfMass();
                    float leaderDistance = Vector2.DistanceSquared(Leader.Position, CurrentWaypoint.Value);
                    int unitsInMotion = 0;

                    for (int i = 0; i < Units.Count; i++)
                    {
                        // Skip units who have "arrived"
                        UnitState unitState = Units[i].GetComponent<UnitState>();
                        if (unitState.CurrentState == UnitStates.Idle)
                        {
                            continue;
                        }

                        // Gather velocities and distances
                        float unitDistance = Vector2.DistanceSquared(Units[i].Position, CurrentWaypoint.Value);
                        Vector2 cohesionVelocity = GetCohesionVelocity(Units[i], centerOfMass);
                        Vector2 destinationVelocity = GetDestinationVelocity(Units[i], centerOfMass);
                        var unitVelocity = destinationVelocity + cohesionVelocity; // + avoidanceVelocity, neighborVelocity;

                        // Check if units has arrived
                        if (leaderDistance <= _arrivalThreshold || unitDistance <= _arrivalThreshold)
                        {
                            unitVelocity = Vector2.Zero;
                            unitState.CurrentState = UnitStates.Idle;
                        }

                        // Update unit's velocity
                        Units[i].GetComponent<Mover>().Velocity = unitVelocity;

                        // Track number of units currently in motion
                        unitsInMotion++;
                    }

                    // All units have arrived
                    if (unitsInMotion == 0)
                    {
                        // Follow the next waypoint
                        if (GetNextWaypoint())
                        {
                            SetStateToMoving();
                        }
                        // Group up
                        else
                        {
                            SetStateToGrouping();
                        }
                    }

                    break;
                case UnitGroupState.Arrived:
                        CurrentState = UnitGroupState.Grouping;
                        CurrentWaypoint = null;
                    break;
                case UnitGroupState.Grouping:
                    int unitsGrouping = 0;

                    for (int i = 0; i < Units.Count; i++)
                    {
                        // Skips units who have arrived
                        if (Units[i].GetComponent<UnitState>().CurrentState == UnitStates.Idle)
                        {
                            continue;
                        }

                        float distanceToLeader = Vector2.Distance(Leader.Position, Units[i].Position);
                        if (distanceToLeader > StopDistance)
                        {
                            Vector2 cohesionVelocity = GetGroupingVelocity(Units[i]);
                            Units[i].GetComponent<Mover>().Velocity = cohesionVelocity;
                            unitsGrouping ++;
                        }
                        else
                        {
                            Units[i].GetComponent<Mover>().Velocity = Vector2.Zero;
                            Units[i].GetComponent<UnitState>().CurrentState = UnitStates.Idle;
                        }

                    }

                    if (unitsGrouping == 0)
                    {
                        CurrentState = UnitGroupState.Idle;
                    }
                    break;
                case UnitGroupState.Idle:
                    if (GetNextWaypoint())
                    {
                        SetStateToMoving();
                    }
                    break;
            }
        }

        private bool GetNextWaypoint()
        {
            if (Waypoints.Count > 0)
            {
                CurrentWaypoint = Waypoints.Dequeue();
                return true;
            }

            return false;
        }

        private Vector2 GetCenterOfMass()
        {
            var center = Vector2.Zero;
            for (int i = 0; i < Units.Count; i++)
            {
                center += Units[i].Position;
            }
            return Vector2.Divide(center, Units.Count);
        }

        private Vector2 GetCohesionVelocity(Entity unit, Vector2 centerOfMass)
        {
            var unitPos = unit.GetComponent<CircleCollider>().Center;
            Vector2 cohVelocity = (centerOfMass - unitPos) * _cohesionFactor;
            return LimitVelocity(cohVelocity, _cohesionVelocityLimit);
        }

        private Vector2 GetAvoidanceVelocity(Entity unit)
        {
            var avoidV = Vector2.Zero;
            Vector2 unitPos = unit.GetComponent<CircleCollider>().Center;

            for (int i = 0; i < Units.Count; i++)
            {
                Vector2 unitIPos = Units[i].GetComponent<CircleCollider>().Center;
                if (unit != Units[i])
                {
                    float distanceSqrd = Vector2.DistanceSquared(unitPos, unitIPos);
                    if (distanceSqrd < _maxDistanceSqrd)
                    {
                        avoidV = avoidV - (unitPos - unitIPos);
                    }
                }
            }

            return avoidV;
        }

        private Vector2 GetDestinationVelocity(Entity unit, Vector2 centerOfMass)
        {
            Vector2 velocity = Vector2.Zero;
            if (CurrentWaypoint == null)
            {
                return velocity;
            }

            float distanceFromLeader = Vector2.DistanceSquared(Leader.Position, unit.Position);

            if (distanceFromLeader > _followLeaderDist)
            {
                velocity = CurrentWaypoint.Value - unit.Position;
            }
            else
            {
                velocity = CurrentWaypoint.Value - Leader.Position;
            }

            
            return Vector2.Normalize(velocity) * _destinationFactor;
        }

        private Vector2 GetGroupingVelocity(Entity unit)
        {
            return Leader.Position - unit.Position;
        }

        private Vector2 LimitVelocity(Vector2 currentVelocity, float maxDistance)
        {
            if (currentVelocity.Length() > maxDistance)
            {
                return Vector2.Normalize(currentVelocity) * maxDistance;
            }

            return currentVelocity;
        }

        private void SetFollowLeaderDistance()
        {
            _followLeaderDist = _followLeaderBase * Units.Count;
        }

        private void SetStateToMoving()
        {
            CurrentState = UnitGroupState.Moving;
            SetUnitsToRunning();
        }

        private void SetStateToGrouping()
        {
            CurrentState = UnitGroupState.Grouping;
            CurrentWaypoint = null;
            StopDistance = GetStopDistance();
            SetUnitsToRunning();
        }

        private void SetUnitsToRunning()
        {
            for (int i = 0; i < Units.Count; i++)
            {
                Units[i].GetComponent<UnitState>().CurrentState = UnitStates.Running;
            }
        }

        private float GetStopDistance()
        {
            int c = Units.Count;
            double r = Math.Pow(Units[0].GetComponent<CircleCollider>().Radius, 2);

            return (float)Math.Sqrt(c * r * _circlePackingConst) + 5;
        }
    }
}
