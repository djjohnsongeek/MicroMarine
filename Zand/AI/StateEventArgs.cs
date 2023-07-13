using System;

namespace Zand.AI
{
    public class StateEventArgs : EventArgs
    {
        public StateEventArgs(StateEventType eventType)
        {
            EventType = eventType;


        }
        public StateEventType EventType { get; private set; }
    }

    public enum StateEventType
    {
        Enter,
        Exit
    }
}
