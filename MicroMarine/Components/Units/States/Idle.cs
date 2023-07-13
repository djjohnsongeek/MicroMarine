using Microsoft.Xna.Framework;
using Zand;
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
            SetUnitAnimation("Idle");
        }

        public override void Update()
        {
            var nextCommand = _unitCommands.Peek();

            if (nextCommand is null)
            {
                if (TargetsAreNearby(out Entity nextTarget))
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
                case CommandType.Attack:
                    _machine.ChangeState<Attacking>();
                    break;
                default:
                    break;
            }
        }
    }
}
