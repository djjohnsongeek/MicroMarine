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
                Velocity = _currentWaypoint.Value - Entity.Position;
                Velocity.Normalize();
                Entity.Position += Vector2.Multiply(Velocity, _speed * (float)Time.DeltaTime);
                float distance = Vector2.Distance(Entity.Position, _currentWaypoint.Value);

                if (distance < .5f)
                {
                    Entity.Position = _currentWaypoint.Value;
                    StopMovement();
                }
            }
        }

        public void StopMovement()
        {
            _currentWaypoint = null;
            Velocity = Vector2.Zero;
        }
    }
}
