using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand;

namespace Zand.ECS.Components
{
    public class WaypointMovement : Component, Zand.IUpdateable
    {
        private float _speed;
        private const float _arrivalDiff = 0.5F;
        private Vector2? _currentWaypoint;

        public Vector2 Velocity;

        public WaypointMovement(float speed)
        {
            _currentWaypoint = null;
            _speed = speed;
            Velocity = Vector2.Zero;
        }

        public void Update()
        {
            UpdateCurrentWaypoint();

            if (_currentWaypoint.HasValue)
            {
                CalculateVelocity();
                ApplyVelocity();
                if (ArrivedAtWaypoint())
                {
                    Arrive();
                }
            }

            UpdateEntityLayerDepth();
        }

        private void CalculateVelocity()
        {
           Velocity = Vector2.Subtract(_currentWaypoint.Value, Entity.Position);
           Velocity.Normalize();
        }

        public void ApplyVelocity()
        {
            Entity.Position += Vector2.Multiply(Velocity, _speed * (float)Time.DeltaTime);
        }

        public void Nudge(Vector2 velocity)
        {
            Entity.Position += velocity;
        }

        private void Arrive()
        {
            Entity.Position = _currentWaypoint.Value;
            StopMovement();
        }

        public void StopMovement()
        {
            _currentWaypoint = null;
            Velocity = Vector2.Zero;
        }

        private bool ArrivedAtWaypoint()
        {
            float distance = Vector2.DistanceSquared(Entity.Position, _currentWaypoint.Value);
            return distance < _arrivalDiff * _arrivalDiff;
        }

        private void UpdateCurrentWaypoint()
        {
            WaypointNav waypoints = Entity.GetComponent<WaypointNav>();
            if (waypoints.HasWaypoints() && !_currentWaypoint.HasValue)
            {
                _currentWaypoint = waypoints.NextWayPoint();
            }
        }

        private void UpdateEntityLayerDepth()
        {
            Vector2 screenPosition = Scene.Camera.GetScreenLocation(Entity.Position);
            Entity.layerDepth = MathUtil.CalculateLayerDepth(screenPosition.Y, Entity.Dimensions.Y);
        }
    }
}
