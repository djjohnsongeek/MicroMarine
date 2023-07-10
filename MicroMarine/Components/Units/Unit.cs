using Zand;
using Zand.Components;

namespace MicroMarine.Components
{
    class Unit : Component
    {
        public float AttackRange { get; protected set; }
        public float FollowRange { get; protected set; }
        public float Speed { get; protected set; }
        public ushort Damage { get; protected set; }
        public float AttacksPerSecond { get; protected set; }
        public float AttackInterval { get; protected set; }
        public UnitAllegiance Allegiance { get; protected set; }
    }
}
