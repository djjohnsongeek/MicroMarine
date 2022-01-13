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

            if (unitSelection.Selected && Input.RightMouseWasPressed() && Input.KeyIsDown(Keys.LeftShift))
            {
                _waypoints.Enqueue(Scene.Camera.GetWorldLocation(Input.MouseScreenPosition));
                
            }
            else if (unitSelection.Selected && Input.RightMouseWasPressed())
            {
                _waypoints.Clear();
                Entity.GetComponent<UnitMovement>().StopMovement();
                _waypoints.Enqueue(Scene.Camera.GetWorldLocation(Input.MouseScreenPosition));
            }
        }

        public bool HasWaypoints()
        {
            return _waypoints.Count > 0;
        }

        public Vector2 NextWayPoint()
        {
            return _waypoints.Dequeue();
        }
    }
}
