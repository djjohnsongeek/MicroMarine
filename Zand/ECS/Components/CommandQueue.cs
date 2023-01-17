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
            foreach (var command in _commands)
            {
                command.SetStatus(CommandStatus.Completed);
            }
            _commands.Clear();
        }

        public UnitCommand Peek()
        {
            if (_commands.Count == 0)
            {
                return null;
            }
            return _commands.Peek();
        }

        public UnitCommand Dequeue()
        {
            return _commands.Dequeue();
        }

        public int Count => _commands.Count;


    }
}
