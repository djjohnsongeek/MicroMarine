using Microsoft.Xna.Framework;
using System;
using Zand.AI;
using Zand.Components;
using Zand.ECS.Components;
using Zand.Physics;

namespace MicroMarine.Components
{
    class Moving : State<Marine>
    {
        private CommandQueue _unitCommands;

        // waypoints will be needed when movement and commands become more complex
        private MovementQueue _waypoints;
        private Mover _mover;
        private Animator _animator;

        public override void OnInitialize()
        {
            _unitCommands = _context.Entity.GetComponent<CommandQueue>(false);
            _waypoints = _context.Entity.GetComponent<MovementQueue>(false);
            _mover = _context.Entity.GetComponent<Mover>(false);
            _animator = _context.Entity.GetComponent<Animator>(false);
        }

        public override void Exit()
        {

        }

        public override void Enter()
        {
            // Handle commands on another type
            if (_unitCommands.Peek().Type != CommandType.Move)
            {
                throw new NotImplementedException("Movement state cannot handle non movement commands.");
            }
        }

        public override void Update()
        {
            var currentCommand = GetCommand();
            if (UnsupportedCommand(currentCommand))
            {
                return;
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

        private bool UnsupportedCommand(UnitCommand currentCommand)
        {
            bool unsupported = false;

            if (currentCommand is null)
            {
                _machine.ChangeState<Idle>();
                return unsupported;
            }

            switch(currentCommand.Type)
            {
                case CommandType.Attack:
                    unsupported = true;
                    _machine.ChangeState<Attacking>();
                    break;
                case CommandType.Follow:
                    unsupported = true;
                    _machine.ChangeState<Following>();
                    break;
            }

            return unsupported;
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
