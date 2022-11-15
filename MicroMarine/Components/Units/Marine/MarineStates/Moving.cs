using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand.AI;
using Zand.Components;


namespace MicroMarine.Components
{
    class Moving : State<Marine>
    {
        private CommandQueue _unitCommands;
        private MovementQueue _unitDestinations;

        public override void OnInitialize()
        {
            _unitCommands = _context.Entity.GetComponent<CommandQueue>();
            _unitDestinations = _context.Entity.GetComponent<MovementQueue>();
        }

        public override void Exit()
        {

        }

        public override void Enter()
        {
            // determine animation
            // 
        }

        public override void Update()
        {

        }
    }
}
