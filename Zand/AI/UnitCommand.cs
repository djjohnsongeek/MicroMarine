using Microsoft.Xna.Framework;

namespace Zand.AI
{
    public class UnitCommand
    {
        public Entity EntityTarget { get; private set; }
        public Vector2 TargetLocation { get; private set; }
        public CommandType Type { get; private set; }
        public CommandStatus Status { get; private set; }

        public UnitCommand(CommandType type, Entity target, Vector2 targetLocation)
        {
            EntityTarget = target;
            Type = type;
            TargetLocation = targetLocation;
            Status = CommandStatus.Created;
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
