﻿using System;
using Microsoft.Xna.Framework;
using Zand;
using Zand.AI;
using Zand.Colliders;
using Zand.ECS.Components;

namespace MicroMarine.Components
{
    class Attacking: BaseUnitState
    {
        private double _inRangeThreshold = 0.5;
        private double _inRangeDuration = 0;
        private double _elapsedTime = 0;

        public event EventHandler<StateEventArgs> AttackingStateChange;

        public override void Exit()
        {
            _context.Entity.GetComponent<CircleCollider>().Static = false;
            OnAttackingStateChange(new StateEventArgs(StateEventType.Exit));
        }

        public override void Enter()
        {
            _mover.Velocity = Vector2.Zero;
            _context.Entity.GetComponent<CircleCollider>().Static = true;
            OnAttackingStateChange(new StateEventArgs(StateEventType.Enter));
        }

        public override void Update()
        {
            var currentCommand = _unitCommands.Peek();
            if (currentCommand is null || currentCommand.Type != CommandType.Attack)
            {
                _machine.ChangeState<Idle>();
                return;
            }

            if (currentCommand.EntityTarget.IsDestroyed)
            {
                currentCommand.SetStatus(CommandStatus.Completed);
                _unitCommands.Dequeue();
                return;
            }

            if (!TargetIsInRange(currentCommand.EntityTarget, 60))
            {
                _machine.ChangeState<Following>();
                return;
            }
            PlayAttackAnimation(currentCommand.EntityTarget);
            AttackTarget(currentCommand.EntityTarget);
        }

        public bool InRangePeriodIsOver()
        {
            return _inRangeDuration >= _inRangeThreshold;
        }

        public virtual void OnAttackingStateChange(StateEventArgs args)
        {
            AttackingStateChange?.Invoke(this, args);
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
                target.GetComponent<Health>().ApplyDamage(_context.Damage);
            }

            _elapsedTime += Time.DeltaTime;
        }
    }
}
