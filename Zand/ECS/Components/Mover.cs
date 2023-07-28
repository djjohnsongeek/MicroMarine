using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Zand.Assets;
using Zand.Colliders;
using Zand.Components;
using Zand.Debug;

namespace Zand.ECS.Components
{
    public class Mover : Component, IUpdateable
    {
        private float maxSpeed;
        public Vector2 Velocity = Vector2.Zero;
        public UnitDirection Orientation { get; private set; }

        public Mover (float maxSpeed)
        {
            this.maxSpeed = maxSpeed;
            Orientation = UnitDirection.South;
        }

        public void Update()
        {
            Entity.Position += Velocity * (float)Time.DeltaTime;
            Orientation = DetermineUnitDirection(Velocity);
            UpdateEntityLayerDepth();
        }

        public override void OnRemovedFromEntity()
        {
            base.OnRemovedFromEntity();
        }

        public void Nudge(Vector2 velocity)
        {
            Entity.Position += velocity * (float)Time.DeltaTime;
        }

        public void SetPosition(Vector2 newPosition)
        {
            Entity.Position = newPosition;
        }

        private void UpdateEntityLayerDepth()
        {
            Vector2 screenPos = Scene.Camera.GetScreenLocation(Entity.Position);
            Entity.RenderDepth = Calc.CalculateRenderDepth(screenPos.Y, Entity.Dimensions.Y);
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
