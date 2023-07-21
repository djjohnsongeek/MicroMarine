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

            DetermineState(CurrentCommand);
        }
    }
}
