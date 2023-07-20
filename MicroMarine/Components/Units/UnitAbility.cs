using Zand;
using Zand.Components;

namespace MicroMarine.Components.Units
{
    class UnitAbility : Component, Zand.IUpdateable
    {

        protected CoolDown _coolDown;
        public bool OnCoolDown => !_coolDown.Ready;

        public virtual void ExecuteAbility()
        {

        }
    }
}
