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

        }

        public override void Enter()
        {
            // TODO Validate?
        }

        public override void Update()
        {
            var currentCommand = GetCommand();
            if (!SupportedCommand(currentCommand))
            {
                return;
            }


            // check for targets
            if (currentCommand.Type == CommandType.AttackMove)
            {
                var nextTarget = SearchForTarget();
                if (nextTarget != null)
                {
                    var newCommand = new UnitCommand(CommandType.Attack, nextTarget, nextTarget.Position);
                    _unitCommands.InsertCommand(newCommand);
                }
            }


            // check if unit is arrived
            if (UnitArrivedAt(currentCommand.Destination))
            {
                currentCommand.SetStatus(CommandStatus.Completed);
                _mover.Velocity = Vector2.Zero;
                _machine.ChangeState<Idle>();
                currentCommand.Destination.Radius += (float)Math.Cbrt(currentCommand.Destination.Radius);
                _unitCommands.Dequeue();
                return;
            }
            

            Vector2 unitVelocity = Vector2.Normalize(currentCommand.Destination.Position - _context.Entity.Position) * _context.Speed;
            _mover.Velocity = unitVelocity;

            SetMarineAnimation(unitVelocity);
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
                case CommandType.Follow:
                    _machine.ChangeState<Following>();
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

        private UnitCommand GetCommand()
        {
            return _unitCommands.Peek();
        }

        private void SetMarineAnimation(Vector2 velocity)
        {
            string animation = "Walk" + _mover.Orientation.ToString();
            _animator.Play(animation);
        }
    }
}
