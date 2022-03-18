using Microsoft.Xna.Framework;

namespace Zand.ECS.Components
{
    public class WaypointMovement : Component, Zand.IUpdateable
    {
        private float _speed;
        private const float _arrivalDiff = 1F;
        public Vector2? CurrentWayPoint;

        public Vector2 Velocity;

        public WaypointMovement(float speed)
        {
            CurrentWayPoint = null;
            _speed = speed;
            Velocity = Vector2.Zero;
        }

        public void Update()
        {
            UpdateCurrentWaypoint();

            if (CurrentWayPoint.HasValue)
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
           Velocity = Vector2.Subtract(CurrentWayPoint.Value, Entity.Position);
           Velocity.Normalize();
        }

        public void ApplyVelocity()
        {
            Entity.Position += Vector2.Multiply(Velocity, _speed * (float)Time.DeltaTime);
        }

        public void Nudge(Vector2 velocity)
        {
            Entity.Position += Vector2.Multiply(velocity, (float)Time.DeltaTime);
        }

        private void Arrive()
        {
            Entity.Position = CurrentWayPoint.Value;
            StopMovement();
        }

        public void StopMovement()
        {
            CurrentWayPoint = null;
            Velocity = Vector2.Zero;
        }

        private bool ArrivedAtWaypoint()
        {
            float distance = Vector2.DistanceSquared(Entity.Position, CurrentWayPoint.Value);
            return distance < _arrivalDiff * _arrivalDiff;
        }

        private void UpdateCurrentWaypoint()
        {
            WaypointNav waypoints = Entity.GetComponent<WaypointNav>();
            if (waypoints.HasWaypoints() && !CurrentWayPoint.HasValue)
            {
                CurrentWayPoint = waypoints.NextWayPoint();
            }
        }

        private void UpdateEntityLayerDepth()
        {
            Entity.layerDepth = MathUtil.CalculateLayerDepth(Entity.Position.Y, Entity.Dimensions.Y);
        }
    }
}
