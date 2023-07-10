using Zand;
using Zand.AI;
using Zand.Components;

namespace MicroMarine.Components
{
    class Unit : Component
    {
        protected StateMachine<Unit> _stateMachine;

        public float AttackRange { get; protected set; }
        public float SightRange { get; protected set; }
        public float FollowRange { get; protected set; }
        public float Speed { get; protected set; }
        public ushort Damage { get; protected set; }
        public float AttacksPerSecond { get; protected set; }
        public float AttackInterval { get; protected set; }
        public UnitAllegiance Allegiance { get; protected set; }

        public Unit(int allegianceId)
        {
            Allegiance = new UnitAllegiance(allegianceId);
            _stateMachine = new StateMachine<Unit>(this);
        }

        public Unit()
        {

        }

        public void Update()
        {
            _stateMachine.Update();
        }
    }
}
