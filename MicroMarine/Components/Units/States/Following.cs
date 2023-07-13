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
            var currentCommand = _unitCommands.Peek();
            if (currentCommand is null || currentCommand.EntityTarget is null)
            {
                _machine.ChangeState<Idle>();
                return;
            }

            bool targetIsInRange = TargetIsInRange(currentCommand.EntityTarget, 100);

            if (!targetIsInRange)
            {
                Vector2 unitVelocity = Vector2.Normalize(currentCommand.EntityTarget.Position - _context.Entity.Position) * _context.Speed;
                _mover.Velocity = unitVelocity;
                SetUnitAnimation("Walk");
            }

            if (targetIsInRange)
            {
                if (currentCommand.Type == CommandType.Attack)
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
