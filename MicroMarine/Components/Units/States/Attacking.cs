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

        public override void Exit()
        {
            _context.Entity.GetComponent<CircleCollider>().Static = false;

            if (_context is Marine)
            {
                _sfxManger.StopSoundEffect("mShoot", _context.Entity);
            }

            _elapsedTime = 0;
        }

        public override void Enter()
        {
            _mover.Velocity = Vector2.Zero;
            _context.Entity.GetComponent<CircleCollider>().Static = true;

            if (_context is Marine)
            {
                _sfxManger.PlaySoundEffect("mShoot", limitPlayback: false, randomChoice: false, entity: _context.Entity, loop: true);
            }
            _elapsedTime = 0;
        }

        public override void Update()
        {
            base.Update();
            _elapsedTime += Time.DeltaTime;

            if (CurrentCommand is null || CurrentCommand.EntityTarget is null)
            {
                _machine.ChangeState<Idle>();
                return;
            }

            if (CurrentCommand.EntityTarget.IsDestroyed)
            {
                CurrentCommand.SetStatus(CommandStatus.Completed);
                _unitCommands.Dequeue();
                return;
            }

            if (!TargetIsInRange(CurrentCommand.EntityTarget, 60))
            {
                _machine.ChangeState<Following>();
                return;
            }
            PlayAttackAnimation(CurrentCommand.EntityTarget);
            AttackTarget(CurrentCommand.EntityTarget);
        }

        public bool InRangePeriodIsOver()
        {
            return _inRangeDuration >= _inRangeThreshold;
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
                target.GetComponent<Health>().ApplyDamage(_context.Damage);
            }
        }
    }
}
