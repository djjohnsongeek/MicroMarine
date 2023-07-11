using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (_animator.CurrentAnimation.IsSuspended())
            {
                _machine.ChangeState<Idle>();
                return;
            }
        }
    }
}
