using System.Collections.Generic;
using Zand.AI;

namespace Zand.Components
{
    public class CommandQueue: Component
    {
        private LinkedList<UnitCommand> _commands;

        public CommandQueue()
        {
            _commands = new LinkedList<UnitCommand>();
        }

        public void AddCommand(UnitCommand command)
        {
            _commands.AddFirst(command);
        }

        public void InsertCommand(UnitCommand command)
        {
            _commands.AddLast(command);
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
            return _commands.Last.Value;
        }

        public void Dequeue()
        {
            _commands.RemoveLast();
        }

        public int Count => _commands.Count;

        public override void OnRemovedFromEntity()
        {
            _commands.Clear();
            _commands = null;
            base.OnRemovedFromEntity();
        }
    }
}
