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
            MouseSelectCollider unitSelection = Entity.GetComponent<MouseSelectCollider>();

            if (unitSelection.Selected && Input.RightMouseWasPressed())
            {
                Vector2 destination = Scene.Camera.GetWorldLocation(Input.MouseScreenPosition);
                // If this waypoint was just inserted, ignore it
                if (_lastInserted.HasValue && _lastInserted == destination)
                {
                    return;
                }
                // Shift click == queue command
                if (Input.KeyIsDown(Keys.LeftShift))
                {
                    AddWayPoint(destination);
                }
                // Otherwise overwrite previous commands
                else if (destination != Scene.Camera.GetScreenLocation(Entity.Position))
                {
                    _waypoints.Clear();
                    Entity.GetComponent<UnitMovement>().StopMovement();
                    AddWayPoint(destination);
                }
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

        private void AddWayPoint(Vector2 wp)
        {
            _lastInserted = wp;
            _waypoints.Enqueue(wp);
        }
    }
}
