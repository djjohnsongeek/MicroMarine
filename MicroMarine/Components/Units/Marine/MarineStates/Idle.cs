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
        private CommandQueue _unitCommands;
        private Animator _animator;

        public override void OnInitialize()
        {
            _unitCommands = _context.Entity.GetComponent<CommandQueue>(false);
            _animator = _context.Entity.GetComponent<Animator>(false);
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
