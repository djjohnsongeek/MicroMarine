using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zand.Components
{
    class UnitController : Component, IUpdateable
    {
        private bool _selected = false;
        public bool Selected
        {
            get => _selected;
        }

        private Queue<Vector2> _waypoints = new Queue<Vector2>();

        public void Update()
        {

        }
    }
}
