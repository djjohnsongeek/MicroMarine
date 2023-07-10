using Microsoft.Xna.Framework;
using Zand;
using Zand.AI;
using Zand.Components;
using Zand.ECS.Components;

namespace MicroMarine.Components
{
    class BaseMarineState : State<Unit>
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

        protected Entity SearchForTarget()
        {
            var entitiesInRange = _context.Scene.Physics.GetEntitiesWithin(_context.Entity.Position, _context.AttackRange);
            var testDistance = float.MaxValue;
            Entity newTarget = null;

            foreach (var entity in entitiesInRange)
            {
                if (entity.IsDestroyed)
                {
                    continue;
                }

                if (entity.Name == "marine")
                {
                    var allegiance = entity.GetComponent<UnitAllegiance>();

                    if (allegiance.Id != _context.Entity.GetComponent<UnitAllegiance>().Id)
                    {
                        float distance = Vector2.Distance(entity.Position, _context.Entity.Position);
                        if (distance < testDistance)
                        {
                            testDistance = distance;
                            newTarget = entity;
                        }
                    }
                }
            }

            return newTarget;
        }
    }
}
