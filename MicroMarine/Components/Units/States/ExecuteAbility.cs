using MicroMarine.Components.Units;
using Zand.AI;

namespace MicroMarine.Components
{
    class ExecuteAbility : BaseUnitState
    {
        public ExecuteAbility()
        {

        }

        public override void Enter()
        {

        }

        public override void Update()
        {
            base.Update();
            if (CurrentCommand is null || CurrentCommand.Type != CommandType.UseAbility)
            {
                _machine.ChangeState<Idle>();
                return;
            }

            var ability = _context.Entity.GetComponent<UnitAbility>();
            ability.ExecuteAbility();
            CurrentCommand.SetStatus(CommandStatus.Completed);
            _unitCommands.Dequeue();
            _machine.ChangeState<Idle>();
        }
    }
}
