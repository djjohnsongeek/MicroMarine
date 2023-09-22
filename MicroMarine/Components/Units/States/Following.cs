using Microsoft.Xna.Framework;
using System;
using Zand;
using Zand.AI;

namespace MicroMarine.Components
{
    class Following : BaseUnitState
    {
        public override void Exit()
        {

        }

        public override void Update()
        {
            base.Update();
            if (CurrentCommand is null || CurrentCommand.EntityTarget is null)
            {
                _machine.ChangeState<Idle>();
                return;
            }

            bool targetIsInRange = TargetIsInRange(CurrentCommand.EntityTarget);

            if (!targetIsInRange)
            {
                _mover.Velocity = GetUnitVelocity(_context.Entity);
                SetUnitAnimation("Walk");
            }

            if (targetIsInRange)
            {
                if (CurrentCommand.Type == CommandType.Attack)
                {
                    _machine.ChangeState<Attacking>();
                }
                else
                {
                    _mover.Velocity = Vector2.Zero;
                    SetUnitAnimation("Idle");
                }
            }
        }
    }
}
