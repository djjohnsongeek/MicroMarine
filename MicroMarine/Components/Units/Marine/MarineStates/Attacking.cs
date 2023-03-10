using System;
using Microsoft.Xna.Framework;
using Zand;
using Zand.AI;
using Zand.Components;
using Zand.ECS.Components;

namespace MicroMarine.Components
{
    class Attacking: State<Marine>
    {
        private CommandQueue _unitCommands;
        private Animator _animator;
        private Mover _mover;
        private int _maxInRangeCount = 600;
        private int _inRangeCount = 0;

        public override void OnInitialize()
        {
            _unitCommands = _context.Entity.GetComponent<CommandQueue>(false);
            _animator = _context.Entity.GetComponent<Animator>(false);
            _mover = _context.Entity.GetComponent<Mover>(false);
        }

        public override void Exit()
        {

        }

        public override void Update()
        {
            var currentCommand = _unitCommands.Peek();
            if (currentCommand is null || currentCommand.Type != CommandType.Attack)
            {
                _machine.ChangeState<Idle>();
                return;
            }

            if (TargetIsInRange(currentCommand.EntityTarget))
            {
                _inRangeCount++;
                if (InRangePeriodIsOver())
                {
                    _mover.Velocity = Vector2.Zero;
                    // attack animation
                    PlayAttachAnimation(currentCommand.EntityTarget);
                }
            }
            else
            {
                Vector2 unitVelocity = Vector2.Normalize(currentCommand.EntityTarget.Position - _context.Entity.Position) * _context.Speed;
                _mover.Velocity = unitVelocity;
                SetMarineAnimation(_mover.Velocity, "Walk");
                _inRangeCount = 0;
            }
        }

        public bool TargetIsInRange(Entity target)
        {
            var distanceSquared = Vector2.DistanceSquared(_context.Entity.Position, target.Position);
            return distanceSquared < Math.Pow(_context.Range, 2d);
        }

        public bool InRangePeriodIsOver()
        {
            return _inRangeCount >= _maxInRangeCount;
        }

        private void PlayAttachAnimation(Entity target)
        {
            var targetPosition = target.Position;
            var entityPosition = _context.Entity.Position;

            var attackVector = targetPosition - entityPosition;
            var direction = GetAttackOrientation(attackVector);

            _animator.Play($"Attack{direction}");
        }

        private UnitDirection GetAttackOrientation(Vector2 attackVector)
        {
            attackVector.Normalize();
            float dot = Vector2.Dot(Vector2.UnitX, attackVector);
            UnitDirection orientation = UnitDirection.North;
            // close to zero, traveling up or down
            if (dot > -0.5F && dot < 0.5F)
            {

                if (attackVector.Y < 0)
                {
                    orientation = UnitDirection.North;
                }
                else if (attackVector.Y > 0)
                {
                    orientation = UnitDirection.South;
                }
            }
            // close to 1 traveling more horizontal
            if (dot < -0.5 || dot > 0.5F)
            {
                if (attackVector.X > 0)
                {
                    orientation = UnitDirection.East;

                }
                else if (attackVector.X < 0)
                {
                    orientation = UnitDirection.West;
                }
            }

            return orientation;
        }

        private void SetMarineAnimation(Vector2 velocity, string animationVerb)
        {
            string animation = animationVerb + _mover.Orientation.ToString();
            _animator.Play(animation);
        }
    }
}
