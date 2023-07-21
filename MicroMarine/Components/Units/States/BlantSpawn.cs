namespace MicroMarine.Components
{
    class BlantSpawn : BaseUnitState
    {

        public override void Enter()
        {
            _animator.Play("SpawnSouth");
        }

        public override void Update()
        {
            base.Update();

            if (_animator.CurrentAnimation.IsSuspended())
            {
                _machine.ChangeState<Idle>();
                return;
            }
        }
    }
}
