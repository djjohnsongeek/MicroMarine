using Microsoft.Xna.Framework;
using System;
using Zand;
using Zand.AI;
using Zand.Components;
using Zand.Physics;

namespace MicroMarine.Components
{
    class Moving : BaseUnitState
    {
        public override void Exit()
        {
            if (_context is Marine)
            {
                _sfxManger.StopSoundEffect("footstep", _context.Entity);
            }
        }

        public override void Enter()
        {
            if (_context is Marine)
            {
                _sfxManger.PlaySoundEffect("footstep", false, true, _context.Entity, true);
            }
        }

        public override void Update()
        {
            base.Update();

            if (!SupportedCommand(CurrentCommand))
            {
                _machine.ChangeState<Idle>();
                return;
            }


            // check for targets
            if (CurrentCommand.Type == CommandType.AttackMove)
            {
                if (TargetsAreNearby(out Entity nextTarget))
                {
                    var newCommand = new UnitCommand(CommandType.Attack, nextTarget, nextTarget.Position);
                    _unitCommands.InsertCommand(newCommand);
                }
            }


            // check if unit is arrived
            if (UnitArrivedAt(CurrentCommand.Destination))
            {
                CurrentCommand.SetStatus(CommandStatus.Completed);
                _mover.Velocity = Vector2.Zero;
                _machine.ChangeState<Idle>();
                CurrentCommand.Destination.Radius += (float)Math.Cbrt(CurrentCommand.Destination.Radius);
                _unitCommands.Dequeue();
                return;
            }




            Vector2 unitVelocity = Vector2.Normalize(CurrentCommand.Destination.Position - _context.Entity.Position) * _context.Speed;
            _mover.Velocity = unitVelocity;
            SetUnitAnimation("Walk");
        }

        private bool SupportedCommand(UnitCommand currentCommand)
        {
            if (currentCommand is null)
            {
                _machine.ChangeState<Idle>();
                return false;
            }

            bool supported = false;
            switch(currentCommand.Type)
            {
                case CommandType.Attack:
                    _machine.ChangeState<Attacking>();
                    break;
                case CommandType.AttackMove:
                case CommandType.Move:
                    supported = true;
                    break;
            }

            return supported;
        }

        private bool UnitArrivedAt(Destination destination)
        {
            return Collisions.CircleToPoint(destination, _context.Entity.Position);
        }
    }
}
