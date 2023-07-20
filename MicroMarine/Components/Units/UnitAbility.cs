using Zand;
using Zand.Components;

namespace MicroMarine.Components.Units
{
    class UnitAbility : Component, Zand.IUpdateable
    {

        protected CoolDown CoolDown;
        public bool OnCoolDown => !CoolDown.Ready;

        public UnitAbility(float coolDownDur)
        {
            CoolDown = new CoolDown(coolDownDur);
        }

        public virtual void ExecuteAbility()
        {
            CoolDown.Start();
        }

        public override void OnRemovedFromEntity()
        {
            CoolDown = null;
        }

        public virtual void Update()
        {
            CoolDown.Update();
        }
    }
}
