using Microsoft.Xna.Framework;
using Zand.AI;

namespace MicroMarine.Components
{
    class Idle : BaseUnitState
    {

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
                case CommandType.AttackMove:
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
    }
}
