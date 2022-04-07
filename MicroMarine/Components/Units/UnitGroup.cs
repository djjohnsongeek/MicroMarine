using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Zand;
using Zand.ECS.Components;

namespace MicroMarine.Components.Units
{
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

        private float _followLeaderDist = 0;
        private static int _followLeaderBase = 255;
        private static float _matchFactor = 0.125f;
        private static float _cohesionFactor = .5f;
        private static float _avoidFactor = .8f;
        private static int _maxDistanceSqrd = 25 * 25;
        private static float _unitSpeed = .1F;
        private static float _arrivalThreshold = 1;
        private static float _destinationFactor = 100F;
        private static float _cohesionVelocityLimit = 20F;
        private static float _allGroupingTimeLimit = .2F;
        private static float _allGroupingClock = 0;
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
            switch (CurrentState)
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
                        var unitVelocity = GetDestinationVelocity(Units[i]) + GetCohesionVelocity(Units[i], centerOfMass);

                        // Check if units has arrived
                        if (leaderDistance <= _arrivalThreshold)
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
                    // TODO clean this up
                    if (CurrentWaypoint != null || Waypoints.Count > 0)
                    {
                        SetStateToMoving();
                    }

                    int unitsGrouping = 0;

                    for (int i = 0; i < Units.Count; i++)
                    {
                        // Skips units who have arrived
                        if (Units[i].GetComponent<UnitState>().CurrentState == UnitStates.Idle || Units[i].Id == Leader.Id)
                        {
                            continue;
                        }

                        float distanceToLeader = Vector2.Distance(Leader.Position, Units[i].Position);
                        if (distanceToLeader > StopDistance || IsAllGroupingPhase())
                        {
                            Units[i].GetComponent<Mover>().Velocity = GetGroupingVelocity(Units[i]);
                            unitsGrouping++;
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
                        RemoveStatic(Leader);
                        _allGroupingClock = 0;
                    }

                    _allGroupingClock += (float)Time.DeltaTime;
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

        private Vector2 GetDestinationVelocity(Entity unit)
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
            SetUnitStatic(Leader);
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
            if (Units.Count < 4)
            {
                return 19;
            }

            int c = Units.Count;
            double r = Math.Pow(Units[0].GetComponent<CircleCollider>().Radius, 2);

            return (float)Math.Sqrt(c * r * _circlePackingConst);
        }

        private void SetUnitStatic(Entity unit)
        {
            unit.GetComponent<CircleCollider>().Static = true;
            unit.GetComponent<UnitState>().CurrentState = UnitStates.Idle;
        }

        private void RemoveStatic(Entity unit)
        {
            unit.GetComponent<CircleCollider>().Static = false;
        }

        private bool IsAllGroupingPhase()
        {
            return _allGroupingClock < _allGroupingTimeLimit;
        }
    }
}
