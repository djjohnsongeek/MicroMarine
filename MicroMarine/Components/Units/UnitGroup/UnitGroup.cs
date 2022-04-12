using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Zand;
using Zand.AI;
using Zand.ECS.Components;

namespace MicroMarine.Components.UnitGroups
{
    public class UnitGroup
    {
        // JUST FOR DEBUG
        public Scene _scene;
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
        internal static float _arrivalThreshold = 1;
        private static float _destinationFactor = 100F;
        private static float _cohesionVelocityLimit = 20F;
        private static float _allGroupingTimeLimit = .2F;
        internal static float _groupingTimeLimit = 2F;
        internal float _groupingClock = 0;
        private static float _circlePackingConst = 1.1026577908435840990226529966259F;

        internal float StopDistance = 0;
        private StateMachine<UnitGroup> _stateMachine;

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

            // Init states
            _stateMachine = new StateMachine<UnitGroup>(this);
            _stateMachine.AddState(new Idle());
            _stateMachine.AddState(new Moving());
            _stateMachine.AddState(new Grouping());
            _stateMachine.SetInitialState<Moving>();
        }

        public void AssignNewLeader()
        {
            Vector2 centerOfMass = GetCenterOfMass();
            float distance = float.MaxValue;
            if (Leader != null)
            {
                RemoveStatic(Leader);
            }

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
            _stateMachine.Update();
        }

        public bool IsStale()
        {
            return _stateMachine.CurrentState is Idle || Units.Count == 0;
        }

        internal bool GetNextWaypoint()
        {
            if (Waypoints.Count > 0)
            {
                CurrentWaypoint = Waypoints.Dequeue();
                return true;
            }

            return false;
        }

        internal Vector2 GetCenterOfMass()
        {
            var center = Vector2.Zero;
            for (int i = 0; i < Units.Count; i++)
            {
                center += Units[i].Position;
            }
            return Vector2.Divide(center, Units.Count);
        }

        internal Vector2 GetCohesionVelocity(Entity unit, Vector2 centerOfMass)
        {
            var unitPos = unit.GetComponent<CircleCollider>().Center;
            Vector2 cohVelocity = (centerOfMass - unitPos) * _cohesionFactor;
            return LimitVelocity(cohVelocity, _cohesionVelocityLimit);
        }

        internal Vector2 GetDestinationVelocity(Entity unit)
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

        internal Vector2 GetGroupingVelocity(Entity unit)
        {
            return Leader.Position - unit.Position;
        }

        internal Vector2 LimitVelocity(Vector2 currentVelocity, float maxDistance)
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

        public void SetUnitsToRunning()
        {
            for (int i = 0; i < Units.Count; i++)
            {
                Units[i].GetComponent<UnitState>().CurrentState = UnitStates.Running;
            }
        }

        internal float GetStopDistance()
        {
            if (Units.Count < 4)
            {
                return 19;
            }

            int c = Units.Count;
            double r = Math.Pow(Units[0].GetComponent<CircleCollider>().Radius, 2);

            return (float)Math.Sqrt(c * r * _circlePackingConst);
        }

        internal void SetUnitStatic(Entity unit)
        {
            unit.GetComponent<CircleCollider>().Static = true;
            unit.GetComponent<UnitState>().CurrentState = UnitStates.Idle;
        }

        internal void RemoveStatic(Entity unit)
        {
            unit.GetComponent<CircleCollider>().Static = false;
        }

        internal bool IsAllGroupingPhase()
        {
            return _groupingClock < _allGroupingTimeLimit;
        }

        internal bool ReachedGroupingTimeLimit()
        {
            return _groupingClock >= _groupingTimeLimit;
        }

        internal bool ShouldGroup(float distanceToLeader)
        {
            return (distanceToLeader > StopDistance || IsAllGroupingPhase()) && !ReachedGroupingTimeLimit();
        }
    }
}
