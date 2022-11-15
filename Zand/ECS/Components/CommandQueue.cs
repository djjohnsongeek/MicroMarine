using System.Collections.Generic;
using Zand.AI;

namespace Zand.Components
{
    public class CommandQueue: Component
    {
        private Queue<UnitCommand> _commands;

        public UnitCommand PeekNext()
        {
            if (_commands.Count == 0)
            {
                return null;
            }
            else
            {
                return _commands.Peek();
            }
        }

        public CommandQueue()
        {
            _commands = new Queue<UnitCommand>();
        }
    }
}
