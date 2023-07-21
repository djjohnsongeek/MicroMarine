using Microsoft.Xna.Framework;
using System;
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
            base.Update();
            if (CurrentCommand is null)
            {
                if (TargetsAreNearby(out Entity nextTarget))
                {
                    CurrentCommand = new UnitCommand(CommandType.Attack, nextTarget, nextTarget.Position);
                    _unitCommands.AddCommand(CurrentCommand);
                }
                else
                {
                    return;
                }
            }

            switch(CurrentCommand.Type)
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
    }
}
