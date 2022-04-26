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
        //

        public string Id;
        public List<Entity> Units;
        public Entity Leader = null;
        public Queue<Vector2> Waypoints;
        public Vector2? CurrentWaypoint;
        internal float GroupingClock = 0;
        internal float StopDistance = 0;

        private float _followLeaderDist = 0;
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
            Vector2 cohVelocity = (centerOfMass - unitPos) * Core.Config.CohesionFactor;
            return LimitVelocity(cohVelocity, Core.Config.CohesionVelocityLimit);
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


            return Vector2.Normalize(velocity) * Core.Config.DestinationFactor;
        }

        internal Vector2 GetGroupingVelocity(Entity unit)
        {
            return LimitVelocity(Leader.Position - unit.Position, Core.Config.DestinationFactor);
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
            _followLeaderDist = Core.Config.FollowLeaderBaseDistance * Units.Count;
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

            return (float)Math.Sqrt(c * r * Core.Config.CirclePackingConst);
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
            return GroupingClock < Core.Config.AllGroupingTimeLimit;
        }

        internal bool ReachedGroupingTimeLimit()
        {
            return GroupingClock >= Core.Config.GroupingTimeLimit;
        }

        internal bool ShouldGroup(float distanceToLeader)
        {
            return (distanceToLeader > StopDistance || IsAllGroupingPhase()) && !ReachedGroupingTimeLimit();
        }
    }
}
