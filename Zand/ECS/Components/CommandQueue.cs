using System.Collections.Generic;
using Zand.AI;

namespace Zand.Components
{
    public class CommandQueue: Component
    {
        private Queue<UnitCommand> _commands;
        public UnitCommand CurrentCommand { get; private set; }

        public UnitCommand PeekNext()
        {
            if (_commands.Count == 0)
            {
                return null;
            }
            return _commands.Peek();
        }

        public UnitCommand Next()
        {
            if (_commands.Count == 0)
            {
                CurrentCommand = null;
            }
            else
            {
                CurrentCommand = _commands.Dequeue();
            }

            return CurrentCommand;
        }

        public CommandQueue()
        {
            _commands = new Queue<UnitCommand>();
        }
    }
}
