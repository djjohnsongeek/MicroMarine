using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand;

namespace MicroMarine.Components
{
    class UnitMovement : Component, Zand.IUpdateable
    {
        private float _speed;
        private Vector2? _currentWaypoint;

        public Vector2 Velocity;

        public UnitMovement(float speed)
        {
            _currentWaypoint = null;
            SetSpeed(speed);
            Velocity = Vector2.Zero;
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        public void Update()
        {
            WaypointNav waypoints = Entity.GetComponent<WaypointNav>();
            if (waypoints.HasWaypoints() && !_currentWaypoint.HasValue)
            {
                _currentWaypoint = waypoints.NextWayPoint();
            }

            if (_currentWaypoint.HasValue)
            {
                Vector2 waypointDistance = GetWaypointVelocity();
                Entity.Position += Vector2.Multiply(Velocity + waypointDistance, _speed * (float)Time.DeltaTime);

                float distance = Vector2.Distance(Entity.Position, _currentWaypoint.Value);
                if (distance < .5f)
                {
                    Entity.Position = _currentWaypoint.Value;
                    StopMovement();
                }
            }

            Vector2 screenPosition = Scene.Camera.GetScreenLocation(Entity.Position);
            Entity.layerDepth = MathUtil.CalculateLayerDepth(screenPosition.Y, Entity.Dimensions.Y);
        }

        private Vector2 GetWaypointVelocity()
        {
           Vector2 difference = Vector2.Subtract(_currentWaypoint.Value, Entity.Position);
           difference.Normalize();
           return difference;
        }

        public void StopMovement()
        {
            _currentWaypoint = null;
            Velocity = Vector2.Zero;
        }
    }
}
