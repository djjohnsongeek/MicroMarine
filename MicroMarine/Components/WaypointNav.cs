using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Zand;

namespace MicroMarine.Components
{
    class WaypointNav : Component, Zand.IUpdateable
    {
        private Queue<Vector2> _waypoints;
        private Vector2? _lastInserted;
        public WaypointNav()
        {
            _waypoints = new Queue<Vector2>();
            _lastInserted = null;
        }

        public void Update()
        {
            MouseSelector unitSelection = Entity.GetComponent<MouseSelector>();

            if (unitSelection.Selected && Input.RightMouseWasPressed() && Input.KeyIsDown(Keys.LeftShift))
            {
                AddWayPoint(Scene.Camera.GetWorldLocation(Input.MouseScreenPosition));
                
            }
            else if (unitSelection.Selected && Input.RightMouseWasPressed())
            {
                if (Input.MouseScreenPosition == Scene.Camera.GetScreenLocation(Entity.Position))
                {
                    return;
                }

                _waypoints.Clear();
                Entity.GetComponent<UnitMovement>().StopMovement();
                AddWayPoint(Scene.Camera.GetWorldLocation(Input.MouseScreenPosition));
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

        public void AddWayPoint(Vector2 wp)
        {
            _lastInserted = wp;
            _waypoints.Enqueue(wp);
        }
    }
}
