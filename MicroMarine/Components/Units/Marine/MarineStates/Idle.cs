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

        public override void OnInitialize()
        {
            _unitCommands = _context.Entity.GetComponent<CommandQueue>();
        }

        public override void Exit()
        {

        }

        public override void Enter()
        {
            // determine animation
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
