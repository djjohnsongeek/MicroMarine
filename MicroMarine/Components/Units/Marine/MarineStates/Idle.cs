using Microsoft.Xna.Framework;
using Zand;
using Zand.AI;
using Zand.Components;
using Zand.ECS.Components;

namespace MicroMarine.Components
{
    class Idle : State<Marine>
    {
        private CommandQueue _unitCommands;
        private Animator _animator;
        private Mover _mover;

        public override void OnInitialize()
        {
            _unitCommands = _context.Entity.GetComponent<CommandQueue>(false);
            _animator = _context.Entity.GetComponent<Animator>(false);
            _mover = _context.Entity.GetComponent<Mover>(false);
        }

        public override void Exit()
        {

        }

        public override void Enter()
        {
            _mover.Velocity = Vector2.Zero;
            string animation = "Idle" + _mover.Orientation.ToString();
            _animator.Play(animation);
        }

        public override void Update()
        {
            var nextCommand = _unitCommands.Peek();

            if (nextCommand is null)
            {
                //
                var nextTarget = SearchForTarget();
                if (nextTarget != null)
                {
                    nextCommand = new UnitCommand(CommandType.Attack, nextTarget, nextTarget.Position);
                    _unitCommands.AddCommand(nextCommand);
                }
                else
                {
                    return;
                }

            }

            switch(nextCommand.Type)
            {
                case CommandType.Move:
                    _machine.ChangeState<Moving>();
                    break;
                case CommandType.Follow:
                    _machine.ChangeState<Following>();
                    break;
                case CommandType.Attack:
                    _machine.ChangeState<Attacking>();
                    break;
                default:
                    break;
            }
        }
        private Entity SearchForTarget()
        {
            //return null;
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
