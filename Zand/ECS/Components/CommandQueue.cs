using System.Collections.Generic;
using Zand.AI;

namespace Zand.Components
{
    public class CommandQueue: Component
    {
        private Queue<UnitCommand> _commands;

        public CommandQueue()
        {
            _commands = new Queue<UnitCommand>();
        }

        public void AddCommand(UnitCommand command)
        {
            _commands.Enqueue(command);
        }

        public void Clear()
        {
            _commands.Clear();
        }

        public UnitCommand PeekNext()
        {
            if (_commands.Count == 0)
            {
                return null;
            }
            return _commands.Peek();
        }

        public int Count => _commands.Count;


    }
}
