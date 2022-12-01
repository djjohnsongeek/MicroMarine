using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand.AI;
using Zand.Components;

namespace MicroMarine.Components
{
    class Idle : State<Marine>
    {
        private UnitCommand CurrentCommad;
        private Animator _animator;

        public override void OnInitialize()
        {
            CurrentCommad _context
            _animator = _context.Entity.GetComponent<Animator>();
        }

        public override void Exit()
        {

        }

        public override void Enter()
        {
            // determine animation
            _animator.SetAnimation("IdleSouth");
        }

        public override void Update()
        {
            var nextCommand = _unitCommands.PeekNext();

            if (nextCommand is null)
            {
                return;
            }

            switch(nextCommand.Type)
            {
                case CommandType.Move:
                    _machine.ChangeState<Moving>();
                    break;
                default:
                    break;
            }
        }
    }
}
