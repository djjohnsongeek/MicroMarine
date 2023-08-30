using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Zand;
using Zand.AI;
using Zand.Colliders;
using Zand.Components;
using Zand.ECS.Components;

namespace MicroMarine.Components
{
    class BaseUnitState : State<Unit>
    {
        protected CommandQueue _unitCommands;
        protected Mover _mover;
        protected Animator _animator;
        protected SoundEffectManager _sfxManger;
        protected UnitCommand CurrentCommand;

        public override void OnInitialize()
        {
            _unitCommands = _context.Entity.GetComponent<CommandQueue>(false);
            _mover = _context.Entity.GetComponent<Mover>(false);
            _animator = _context.Entity.GetComponent<Animator>(false);
            _sfxManger = _context.Entity.Scene.GetComponent<SoundEffectManager>();
        }

        public override void Update()
        {
            CurrentCommand = _unitCommands.Peek();
        }

        protected bool TargetsAreNearby(out Entity entity)
        {
            var entitiesInRange = _context.Scene.Physics.GetEntitiesWithin(_context.Entity.Position, _context.SightRange);
            entity = GetClosestEnemyUnit(entitiesInRange);
            return entity != null;
        }

        protected bool TargetIsInRange(Entity target, float buffer = 0)
        {
            var distanceSquared = Vector2.DistanceSquared(_context.Entity.Position, target.Position);
            var attackRangeSquared = Math.Pow(_context.AttackRange, 2d);

            return attackRangeSquared - distanceSquared > buffer;
        }

        protected Entity GetClosestEnemyUnit(List<Entity> entities)
        {
            var shortestDistance = float.MaxValue;
            Entity closestEnemy = null;

            foreach (var entity in entities)
            {
                var allegiance = entity.GetComponent<UnitAllegiance>();
                bool sameTeam = allegiance.Id == _context.Entity.GetComponent<UnitAllegiance>().Id;

                if (entity.IsDestroyed || sameTeam || entity.Name != "unit")
                {
                    continue;
                }

                float distance = Vector2.Distance(entity.Position, _context.Entity.Position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestEnemy = entity;
                }
            }

            return closestEnemy;
        }

        protected void SetUnitAnimation(string animationVerb)
        {
            string animation = animationVerb + _mover.Orientation.ToString();
            _animator.Play(animation);
        }

        protected void DetermineState(UnitCommand cmd)
        {
            switch (cmd.Type)
            {
                case CommandType.Move:
                case CommandType.AttackMove:
                    _machine.ChangeState<Moving>();
                    break;
                case CommandType.Attack:
                    _machine.ChangeState<Attacking>();
                    break;
                case CommandType.UseAbility:
                    _machine.ChangeState<ExecuteAbility>();
                    break;
                default:
                    throw new NotImplementedException($"Unknown Command: {CurrentCommand}");
            }
        }

        protected Vector2 GetUnitVelocity(Entity entity)
        {
            var units = _context.Entity.Scene.Physics.GetCollidersWithin(_context.Entity.ScreenPosition, BoidConfig.BoidVision);
            var (friendlies, statics) = SortNearbyUnits(units);

            var cohesionV = GetCohesionVelocity(_context.Entity);
            var seperationV = GetSeperationVelocity(_context.Entity);
            var groupV = GetGroupVelocity(_context.Entity);
            var destinationV = GetDestinationVelocity(_context.Entity);
            var avoidV = GetAvoidVelocity(_context.Entity);


            var unitVelocity = cohesionV + seperationV + groupV + destinationV + avoidV;
            unitVelocity = ClampVelocity(unitVelocity, _mover.MaxSpeed);

            // TODO arrival checks?


            return unitVelocity;
        }


        private (List<CircleCollider> friendlies, List<CircleCollider> statics) SortNearbyUnits(List<CircleCollider> colliders)
        {
            // we are assuming a circle collider means it is a unit ...
            var statics = new List<CircleCollider>();
            var friendlies = new List<CircleCollider>();

            var unitAllegiance = _context.Entity.GetComponent<UnitAllegiance>();

            for (int i = 0; i < colliders.Count; i++)
            {
                if (colliders[i].Static)
                {
                    statics.Add(colliders[i]);
                }

                if (colliders[i].Entity.GetComponent<UnitAllegiance>().Id == unitAllegiance.Id)
                {
                    friendlies.Add(colliders[i]);
                }
            }

            return (friendlies, statics);
        }


        private Vector2 GetCohesionVelocity(Entity e)
        {
            throw new NotImplementedException();
        }

        private Vector2 GetSeperationVelocity(Entity e)
        {
            throw new NotImplementedException();
        }

        private Vector2 GetGroupVelocity(Entity e)
        {
            throw new NotImplementedException();
        }

        private Vector2 GetDestinationVelocity(Entity e)
        {
            throw new NotImplementedException();
        }

        private Vector2 GetAvoidVelocity(Entity e)
        {
            throw new NotImplementedException();
        }

        private Vector2 ClampVelocity(Vector2 velocity, float maxSpeed)
        {
            if (velocity.Length() > maxSpeed)
            {
                velocity.Normalize();
                velocity *= maxSpeed;
            }
            return velocity;
        }
    }
}
