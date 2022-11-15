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
        private Queue<Vector2> _destinations;

        public void AddDesitination(Vector2 destination)
        {
            _destinations.Enqueue(destination);
        }

        public Vector2 Next()
        {
            return _destinations.Dequeue();
        }

        public Vector2 PeekNext()
        {
            return _destinations.Peek();
        }

        public bool IsEmpty()
        {
            return _destinations.Count == 0;
        }

    }
}
