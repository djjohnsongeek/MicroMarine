using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;

namespace Zand.Components
{
    public class MovementQueue : Component
    {
        private Queue<Vector2> _waypoints;
        public Vector2? CurrentWaypoint { get; private set; }

        public void AddWaypoint(Vector2 destination)
        {
            _waypoints.Enqueue(destination);
        }

        public Vector2? Next()
        {
            if (_waypoints.Count == 0)
            {
                CurrentWaypoint = null;
            }
            else
            {
                CurrentWaypoint = _waypoints.Dequeue();
            }
            return CurrentWaypoint;
        }

        public Vector2? PeekNext()
        {
            if (_waypoints.Count == 0)
            {
                return null;
            }
            return _waypoints.Peek();
        }

        public bool IsEmpty()
        {
            return _waypoints.Count == 0;
        }

    }
}
