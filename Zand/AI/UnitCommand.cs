using Microsoft.Xna.Framework;
using Zand.Colliders;

namespace Zand.AI
{
    public class UnitCommand
    {
        public Entity EntityTarget { get; private set; }
        public Destination Destination { get; private set; }
        public CommandType Type { get; private set; }
        public CommandStatus Status { get; private set; }

        public UnitCommand(CommandType type, Entity target, Vector2 targetLocation)
        {
            EntityTarget = target;
            Type = type;
            Destination = new Destination(targetLocation);
            Status = CommandStatus.Created;
        }

        public void SetStatus(CommandStatus status)
        {
            Status = status;
        }
    }

    public enum CommandType : byte
    {
        Move,
        Attack,
        Follow,
        Harvest
    }

    public enum CommandStatus : byte
    {
        Created,
        Issued,
        Completed,
        Cancelled,
    }
}
