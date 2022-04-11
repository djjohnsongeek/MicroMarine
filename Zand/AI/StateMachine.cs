using System;
using System.Collections.Generic;

namespace Zand.AI
{
    public class StateMachine<T>
    {
        protected T _context;
        protected State<T> _currentState;
        public State<T> CurrentState => _currentState;
        private Dictionary<Type, State<T>> _definedStates;
        private Stack<State<T>> _states;

        public StateMachine(T context)
        {
            _context = context;
            _definedStates = new Dictionary<Type, State<T>>();
            _states = new Stack<State<T>>();
        }

        public void AddState(State<T> newState)
        {
            newState.InitContext(this, _context);
            _definedStates.Add(newState.GetType(), newState);
        }

        public void SetInitialState<R>() where R : State<T>
        {
            var nextState = GetState<R>();
            _states.Push(nextState);
            nextState.Enter();
            _currentState = nextState;
        }

        // Replace current state with a new state
        public void ChangeState<R>() where R : State<T>
        {
            var nextState = GetState<R>();

            _currentState.Exit();
            _states.Pop();
            _states.Push(nextState);
            nextState.Enter();
            _currentState = nextState;
        }

        // Switch to a new state, saving the last state for later
        public void NewState<R>() where R : State<T>
        {
            var newState = GetState<R>();

            _currentState.Exit();
            _states.Push(newState);
            newState.Enter();
            _currentState = newState;
        }

        // Removes the current state, and returns to the previous state
        public void PrevState()
        {
            _currentState.Exit();
            _states.Pop();
            _currentState = _states.Peek();
            _currentState.Enter();
        }

        private State<T> GetState<R>() where R : State<T>
        {
            var stateKey = typeof(R);
            if (!_definedStates.ContainsKey(stateKey))
            {
                throw new IndexOutOfRangeException($"{stateKey.GetType()} has no been defined.");
            }

            return _definedStates[stateKey];
        }

        public void Update()
        {
            _currentState.Update();
        }
    }
}
