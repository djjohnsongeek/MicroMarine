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
        private MovementQueue _waypoints;
        private UnitCommand _currentCommand;

        public override void OnInitialize()
        {
            _unitCommands = _context.Entity.GetComponent<CommandQueue>();
            _waypoints = _context.Entity.GetComponent<MovementQueue>();
        }

        public override void Exit()
        {

        }

        public override void Enter()
        {
            _unitCommands.Next();
            if (_unitCommands.CurrentCommand is null)
            {
                _machine.ChangeState<Idle>();
                return;
            }

            // Handle commands on another type
            if (_unitCommands.CurrentCommand.Type != CommandType.Move)
            {
                throw new NotImplementedException();
            }

            GetWaypoints();
        }

        public override void Update()
        {
           // add way points from command
           // add 
        }

        private void GetWaypoints()
        {
            _waypoints.AddWaypoint(_unitCommands.CurrentCommand.TargetLocation);
        }
    }
}
