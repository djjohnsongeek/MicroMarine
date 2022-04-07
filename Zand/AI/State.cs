namespace Zand.AI
{
    public abstract class State<T>
    {
        protected StateMachine<T> _machine;
        protected T _context;

        public void InitContext(StateMachine<T> machine, T context)
        {
            _context = context;
            _machine = machine;
            OnInitialize();
        }

 
        public virtual void OnInitialize()
        {

        }

        public virtual void Exit()
        {

        }

        public virtual void Enter()
        {

        }

        public virtual void Update()
        {

        }
    }
}
