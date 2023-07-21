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
            var currentCommand = _unitCommands.Peek();
            if (currentCommand is null || currentCommand.Type != CommandType.UseAbility)
            {
                _machine.ChangeState<Idle>();
                return;
            }

            var ability = _context.Entity.GetComponent<UnitAbility>();
            ability.ExecuteAbility();
            currentCommand.SetStatus(CommandStatus.Completed);
            _unitCommands.Dequeue();
            _machine.ChangeState<Idle>();
        }
    }
}
