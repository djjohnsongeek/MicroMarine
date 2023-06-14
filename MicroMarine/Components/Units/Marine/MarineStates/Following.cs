using Microsoft.Xna.Framework;
using System;
using Zand;
using Zand.AI;
using Zand.Components;
using Zand.ECS.Components;

namespace MicroMarine.Components
{
    class Following : State<Marine>
    {
        private CommandQueue _unitCommands;
        private Animator _animator;
        private Mover _mover;
        private double _inRangeThreshold = .5;
        private double _inRangeDuration = 0;

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
            if (currentCommand is null || currentCommand.Type != CommandType.Follow)
            {
                _machine.ChangeState<Idle>();
                return;
            }

            if (TargetIsInRange(currentCommand.EntityTarget))
            {
                _inRangeDuration += Time.DeltaTime;
                if (InRangePeriodIsOver())
                {
                    _mover.Velocity = Vector2.Zero;
                    SetMarineAnimation(_mover.Velocity, "Idle");
                }
            }
            else
            {
                Vector2 unitVelocity = Vector2.Normalize(currentCommand.EntityTarget.Position - _context.Entity.Position) * _context.Speed;
                _mover.Velocity = unitVelocity;
                SetMarineAnimation(_mover.Velocity, "Walk");
                _inRangeDuration = 0;
            }
        }

        public bool TargetIsInRange(Entity target)
        {
            var distanceSquared = Vector2.DistanceSquared(_context.Entity.Position, target.Position);
            return distanceSquared < Math.Pow(_context.FollowRange, 2d);
        }

        public bool InRangePeriodIsOver()
        {
            return _inRangeDuration >= _inRangeThreshold;
        }

        private void SetMarineAnimation(Vector2 velocity, string animationVerb)
        {
            string animation = animationVerb + _mover.Orientation.ToString();
            _animator.Play(animation);
        }
    }
}
