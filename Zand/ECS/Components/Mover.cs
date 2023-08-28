using Microsoft.Xna.Framework;
using Zand.Colliders;
using Zand.Components;

namespace Zand.ECS.Components
{
    public class Mover : Component, IUpdateable
    {
        public float MaxSpeed;
        public Vector2 Velocity = Vector2.Zero;
        public UnitDirection Orientation { get; private set; }
        private CircleCollider _collider;
        private Animator _animator;

        public Mover (float maxSpeed)
        {
            MaxSpeed = maxSpeed;
            Orientation = UnitDirection.South;

        }

        public override void OnAddedToEntity()
        {
            _collider = Entity.GetComponent<CircleCollider>(onlyInitialized: false);
            _animator = Entity.GetComponent<Animator>();
        }

        public void Update()
        {
            Entity.Position += Velocity * (float)Time.DeltaTime;
            Orientation = DetermineUnitDirection(Velocity);
            UpdateEntityLayerDepth();

            if ((Velocity.X > 0 || Velocity.Y > 0) && _collider != null)
            {
                _collider.Dirty = true;
            }
        }

        public override void OnRemovedFromEntity()
        {
            base.OnRemovedFromEntity();
        }

        public void Nudge(Vector2 velocity)
        {
            Entity.Position += velocity * (float)Time.DeltaTime;
            if (_collider != null)
            {
                _collider.Dirty = true;
            }

        }

        public void SetPosition(Vector2 newPosition)
        {
            Entity.Position = newPosition;
            _collider.Dirty = true;
        }

        private void UpdateEntityLayerDepth()
        {
            Vector2 screenPos = Scene.Camera.GetScreenLocation(Entity.Position);
           _animator.RenderDepth = Calc.CalculateRenderDepth(screenPos.Y, Entity.Dimensions.Y);
        }

        public UnitDirection DetermineUnitDirection(Vector2 velocity)
        {
            if (velocity == Vector2.Zero)
            {
                return Orientation;
            }

            return DetermineUnitDirection(Vector2.Zero, velocity);
        }

        public static UnitDirection DetermineUnitDirection(Vector2 agentPosition, Vector2 targetPosition)
        {
            var difference = targetPosition - agentPosition;
            difference.Normalize();

            float dot = Vector2.Dot(Vector2.UnitX, difference);
            UnitDirection orientation = UnitDirection.North;
            // close to zero, traveling up or down
            if (dot > -0.5F && dot < 0.5F)
            {
                if (difference.Y < 0)
                {
                    orientation = UnitDirection.North;
                }
                else if (difference.Y > 0)
                {
                    orientation = UnitDirection.South;
                }
            }
            // close to 1 traveling more horizontal
            if (dot < -0.5 || dot > 0.5F)
            {
                if (difference.X > 0)
                {
                    orientation = UnitDirection.East;

                }
                else if (difference.X < 0)
                {
                    orientation = UnitDirection.West;
                }
            }

            return orientation;
        }
    }



    public enum UnitDirection
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest,
    }
}
