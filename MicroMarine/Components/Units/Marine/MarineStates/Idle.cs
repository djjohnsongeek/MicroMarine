using Zand.AI;
using Zand.Components;
using Zand.ECS.Components;

namespace MicroMarine.Components
{
    class Idle : State<Marine>
    {
        private CommandQueue _unitCommands;
        private Animator _animator;
        private Mover _mover;

        public override void OnInitialize()
        {
            _unitCommands = _context.Entity.GetComponent<CommandQueue>(false);
            _animator = _context.Entity.GetComponent<Animator>(false);
            _mover = _context.Entity.GetComponent<Mover>(false);
        }

        public override void Exit()
        {

        }

        public override void Enter()
        {
            string animation = "Idle" + _mover.Orientation.ToString();
            _animator.Play(animation);
        }

        public override void Update()
        {
            var nextCommand = _unitCommands.Peek();

            if (nextCommand is null)
            {
                return;
            }

            switch(nextCommand.Type)
            {
                case CommandType.Move:
                    _machine.ChangeState<Moving>();
                    break;
                case CommandType.Follow:
                    _machine.ChangeState<Following>();
                    break;
                default:
                    break;
            }
        }
    }
}
