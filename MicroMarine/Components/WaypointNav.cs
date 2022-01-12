using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Zand;

namespace MicroMarine.Components
{
    class WaypointNav : Component, Zand.IUpdateable
    {
        private Queue<Vector2> _waypoints;

        public WaypointNav()
        {
            _waypoints = new Queue<Vector2>();
        }

        public void Update()
        {
            MouseSelector unitSelection = Entity.GetComponent<MouseSelector>();

            if (unitSelection.Selected && Input.RightMouseWasPressed())
            {
                _waypoints.Clear();
                _waypoints.Enqueue(Input.MousePosition);
            }
            else if (unitSelection.Selected && Input.RightMouseWasPressed() && Input.KeyIsDown(Keys.LeftShift))
            {
                _waypoints.Enqueue(Input.MousePosition);
            }
        }
    }
}
