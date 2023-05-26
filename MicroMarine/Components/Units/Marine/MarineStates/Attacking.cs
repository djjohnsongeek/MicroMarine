using System;
using Microsoft.Xna.Framework;
using Zand;
using Zand.AI;
using Zand.Components;
using Zand.ECS.Components;
using Zand.Debug;

namespace MicroMarine.Components
{
    class Attacking: State<Marine>
    {
        private CommandQueue _unitCommands;
        private Animator _animator;
        private Mover _mover;
        private int _maxInRangeCount = 600;
        private int _inRangeCount = 0;
        private double _elapsedTime = 0;


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

            if (TargetIsDead(currentCommand.EntityTarget))
            {
                currentCommand.SetStatus(CommandStatus.Completed);
                _unitCommands.Dequeue();
                return;
            }

            if (TargetIsInRange(currentCommand.EntityTarget))
            {
                _inRangeCount++;
                if (InRangePeriodIsOver())
                {
                    _mover.Velocity = Vector2.Zero;
                    PlayAttackAnimation(currentCommand.EntityTarget);
                    AttackTarget(currentCommand.EntityTarget);
                }
            }
            else
            {
                Vector2 unitVelocity = Vector2.Normalize(currentCommand.EntityTarget.Position - _context.Entity.Position) * _context.Speed;
                _mover.Velocity = unitVelocity;
                _animator.Play($"Walk{_mover.Orientation}");
                _inRangeCount = 0;
            }
        }

        public bool TargetIsInRange(Entity target)
        {
            var distanceSquared = Vector2.DistanceSquared(_context.Entity.Position, target.Position);
            return distanceSquared < Math.Pow(_context.AttackRange, 2d);
        }

        public bool TargetIsDead(Entity target)
        {
            return !target.Enabled;
        }

        public bool InRangePeriodIsOver()
        {
            return _inRangeCount >= _maxInRangeCount;
        }

        private void PlayAttackAnimation(Entity target)
        {
            UnitDirection orientation = Mover.DetermineUnitDirection(_context.Entity.Position, target.Position);
            _animator.Play($"Attack{orientation}");
        }

        private void AttackTarget(Entity target)
        {
            if (_elapsedTime >= _context.AttackInterval)
            {
                _elapsedTime = 0;
            }

            if (_elapsedTime == 0)
            {
                target.GetComponent<Health>().Damage(_context.Damage);
            }

            _elapsedTime += Time.DeltaTime;
        }
    }
}
