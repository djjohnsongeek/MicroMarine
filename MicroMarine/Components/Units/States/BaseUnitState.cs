using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Zand;
using Zand.AI;
using Zand.Components;
using Zand.ECS.Components;

namespace MicroMarine.Components
{
    class BaseUnitState : State<Unit>
    {
        protected CommandQueue _unitCommands;
        protected Mover _mover;
        protected Animator _animator;

        public override void OnInitialize()
        {
            _unitCommands = _context.Entity.GetComponent<CommandQueue>(false);
            _mover = _context.Entity.GetComponent<Mover>(false);
            _animator = _context.Entity.GetComponent<Animator>(false);
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
    }
}
