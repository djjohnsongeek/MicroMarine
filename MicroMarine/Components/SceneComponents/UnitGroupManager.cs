using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Zand;
using Zand.ECS.Components;

namespace MicroMarine.Components
{
    public class UnitGroupManager : SceneComponent
    {
        private List<UnitGroup> UnitGroups;
        private uint IdPool;

        public UnitGroupManager(Scene scene) : base(scene)
        {
            // TODO implement unitgroup pool?
            UnitGroups = new List<UnitGroup>();
            IdPool = 0;
        }

        public override void Update()
        {
            if (Input.RightMouseWasPressed())
            {
                CreateNewUnitGroup();
            }

            UpdateUnitGroups();
        }

        private void UpdateUnitGroups()
        {
            for (int i = UnitGroups.Count - 1; i >= 0; i--)
            {
                // Cull empty or stale unit groups
                if (UnitGroups[i].CurrentState == UnitGroupState.Arrived || UnitGroups[i].Units.Count == 0)
                {
                    UnitGroups.RemoveAt(i);
                    continue;
                }

                UnitGroups[i].Update();
            }
        }

        private void CreateNewUnitGroup()
        {
            // TODO reuse a unit group if it's exactly the same? (just need to update the destination)
            List<Entity> units = Scene.GetComponent<UnitSelector>().GetSelectedUnits();
            Vector2 destination = Scene.Camera.GetWorldLocation(Input.MouseScreenPosition);
            RegisterGroup(new UnitGroup(units, destination));
        }

        private void RegisterGroup(UnitGroup group)
        {
            AssignId(group);
            StealUnits(group);
            UnitGroups.Add(group);

            // really only for debug
            group._scene = Scene;
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

        private void AssignId(UnitGroup group)
        {
            IdPool++;
            group.Id = IdPool;
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
        public uint Id;
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
                        CurrentState = UnitGroupState.Arrived;
                    }

                    break;
                case UnitGroupState.Arrived:
                    if (GetNextWaypoint())
                    {
                        CurrentState = UnitGroupState.Moving;
                    }
                    else
                    {
                        CurrentState = UnitGroupState.Grouping;
                        CurrentWaypoint = null;
                    }
                    break;
                case UnitGroupState.Grouping:
                    break;
                case UnitGroupState.Idle:
                    if (GetNextWaypoint())
                    {
                        CurrentState = UnitGroupState.Moving;
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
                velocity = CurrentWaypoint.Value - Leader.Position; // Leader.Position
            }

            
            return Vector2.Normalize(velocity) * _destinationFactor;
        }

        private Vector2 LimitVelocity(Vector2 currentVelocity, float maxDistance)
        {
            _scene.Debug.Log($"{currentVelocity.Length()}");
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
    }
}
