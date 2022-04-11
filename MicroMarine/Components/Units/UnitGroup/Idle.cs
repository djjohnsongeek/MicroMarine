using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand.AI;
using Microsoft.Xna.Framework;

namespace MicroMarine.Components.UnitGroups
{
    internal class Idle : State<UnitGroup>
    {
        public override void Exit()
        {

        }

        public override void Enter()
        {

        }

        public override void Update()
        {
            if (_context.Waypoints.Count > 0)
            {
                _machine.ChangeState<Moving>();
            }
        }
    }
}
