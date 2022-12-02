using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Zand;
using Zand.AI;
using Zand.Colliders;
using Zand.Components;
using Zand.Utils;

namespace MicroMarine.Components.UnitGroups
{
    public class UnitGroup : IPoolable
    {
        // JUST FOR DEBUG
        public Scene _scene;
        //

        public BitArray Id;
        public List<Entity> Units;
        public Entity Leader = null;
        public Queue<Vector2> Waypoints;
        public CommandQueue GroupCommands;
        internal float GroupingClock = 0;
        internal float StopDistance = 0;

        private float _followLeaderDist = 0;
        private StateMachine<UnitGroup> _stateMachine;

        public UnitGroup()
        {
            Waypoints = new Queue<Vector2>();
            GroupCommands = new CommandQueue();
            Leader = null;
            Units = new List<Entity>();
            InitStates();
            _stateMachine.SetInitialState<Idle>();
        }

        public void Reset()
        {
            Units.Clear();
            RemoveStatic(Leader);
            Waypoints.Clear();
            GroupCommands.Clear();
            Id = null;
            _scene = null;
            GroupingClock = 0;
            StopDistance = 0;
            _followLeaderDist = 0;
            _stateMachine.ChangeState<Idle>();
        }

        public void Setup(BitArray groupId, List<Entity> units, UnitCommand command)
        {
            Id = groupId;
            Units = units;
            GroupCommands.AddCommand(command);
            _stateMachine.ChangeState<Moving>();
            AssignNewLeader();
        }

        private void InitStates()
        {
            _stateMachine = new StateMachine<UnitGroup>(this);
            _stateMachine.AddState(new Idle());
            _stateMachine.AddState(new Moving());
            _stateMachine.AddState(new Grouping());
        }

        public void AssignNewLeader()
        {
            Vector2 centerOfMass = GetCenterOfMass();
            float distance = float.MaxValue;
            RemoveStatic(Leader);

            for (int i = 0; i < Units.Count; i++)
            {
                float unitDistance = Vector2.Distance(Units[i].Position, centerOfMass);
                if (unitDistance < distance)
                {
                    Leader = Units[i];
                    distance = unitDistance;
                }
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
            Vector2 cohVelocity = (centerOfMass - unitPos) * Config.CohesionFactor;
            return LimitVelocity(cohVelocity, Config.CohesionVelocityLimit);
        }

        internal Vector2 GetDestinationVelocity(Entity unit)
        {
            Vector2 velocity = Vector2.Zero;
            if (GroupCommands.CurrentCommand == null)
            {
                return velocity;
            }

            float distanceFromLeader = Vector2.DistanceSquared(Leader.Position, unit.Position);

            if (distanceFromLeader > _followLeaderDist)
            {
                velocity = GroupCommands.CurrentCommand.Destination.Position - unit.Position;
            }
            else
            {
                velocity = GroupCommands.CurrentCommand.Destination.Position - Leader.Position;
            }


            return Vector2.Normalize(velocity) * Config.DestinationFactor;
        }

        internal Vector2 GetGroupingVelocity(Entity unit)
        {
            return LimitVelocity(Leader.Position - unit.Position, Config.DestinationFactor);
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
            _followLeaderDist = Config.FollowLeaderBaseDistance * Units.Count;
        }

        internal float GetStopDistance()
        {
            if (Units.Count <= Config.MaxGroupingSize)
            {
                return Config.MinimumStopDistance;
            }

            int c = Units.Count;
            double r = Math.Pow(Units[0].GetComponent<CircleCollider>().Radius, 2);

            return (float)Math.Sqrt(c * r * Config.CirclePackingConst);
        }

        internal void SetUnitStatic(Entity unit)
        {
            unit.GetComponent<CircleCollider>().Static = true;
        }

        internal void RemoveStatic(Entity unit)
        {
            if (unit == null) return;
            unit.GetComponent<CircleCollider>().Static = false;
        }

        internal bool RemoveUnit(Entity unit)
        {
            return Units.Remove(unit);
        }

        internal bool IsAllGroupingPhase()
        {
            return GroupingClock < Config.AllGroupingTimeLimit;
        }

        internal bool ReachedGroupingTimeLimit()
        {
            return GroupingClock >= Config.GroupingTimeLimit;
        }

        internal bool ShouldGroup(float distanceToLeader)
        {
            return (distanceToLeader > StopDistance || IsAllGroupingPhase()) && !ReachedGroupingTimeLimit();
        }
    }
}
