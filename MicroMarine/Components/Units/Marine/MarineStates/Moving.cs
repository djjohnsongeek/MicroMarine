using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand.AI;
using Zand.Colliders;
using Zand.Components;
using Zand.Debug;
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
            if (currentCommand is null)
            {
                _machine.ChangeState<Idle>();
                return;
            }

            // check if unit is arrived
            if (UnitArrivedAt(currentCommand.Destination))
            {
                currentCommand.SetStatus(CommandStatus.Completed);
                _mover.Velocity = Vector2.Zero;
                _machine.ChangeState<Idle>();
                currentCommand.Destination.Radius += _context.Entity.GetComponent<CircleCollider>().Radius / 1.3f;
                _unitCommands.Dequeue();
                return;
            }
            

            Vector2 unitVelocity = Vector2.Normalize(currentCommand.Destination.Position - _context.Entity.Position) * 100;
            _mover.Velocity = unitVelocity;

            UpdateMarineAnimation(unitVelocity);
        }

        private bool UnitArrivedAt(Destination destination)
        {
            return Collisions.CircleToPoint(destination, _context.Entity.Position);
        }

        private UnitCommand GetCommand()
        {
            return _unitCommands.Peek();
        }

        private void UpdateMarineAnimation(Vector2 velocity)
        {
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
                float dot = Vector2.Dot(Vector2.UnitX, velocity);

                // close to zero, traveling up or down
                if (dot > -0.5F && dot < 0.5F)
                {
                    if (velocity.Y < 0)
                    {
                        if (!_animator.AnimationIsRunning("WalkNorth"))
                        {
                            _animator.SetAnimation("WalkNorth");
                        }
                    }
                    else if (velocity.Y > 0)
                    {
                        if (!_animator.AnimationIsRunning("WalkSouth"))
                        {
                            _animator.SetAnimation("WalkSouth");
                        }
                    }
                }
                // close to 1 traveling more horizontal
                if (dot < -0.5 || dot > 0.5F)
                {
                    if (velocity.X > 0)
                    {
                        if (!_animator.AnimationIsRunning("WalkEast"))
                        {
                            _animator.SetAnimation("WalkEast");
                        }

                    }
                    else if (velocity.X < 0)
                    {
                        if (!_animator.AnimationIsRunning("WalkWest"))
                        {
                            _animator.SetAnimation("WalkWest");
                        }
                    }
                }
            }
            else
            {
                if (!_animator.AnimationIsRunning("IdleSouth"))
                {
                    _animator.SetAnimation("IdleSouth");
                }
            }
        }
    }
}
