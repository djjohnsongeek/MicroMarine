using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand.AI;
using Microsoft.Xna.Framework;
using Zand.Components;

namespace MicroMarine.Components.UnitGroups
{
    internal class Idle : State<UnitGroup>
    {
        private CommandQueue GroupCommands;

        public override void Exit()
        {

        }

        public override void Enter()
        {

        }

        public override void Update()
        {
            if (_context.GroupCommands.PeekNext() != null)
            {
                _machine.ChangeState<Moving>();
            }
        }
    }
}
