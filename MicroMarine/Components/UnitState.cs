using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroMarine.Components
{
    public enum UnitStates
    {
        Idle,
        Running,
    }

    public class UnitState : Zand.Component
    {
        public UnitStates CurrentState;
        public UnitState(UnitStates startingState)
        {
            CurrentState = startingState;
        }
    }
}
