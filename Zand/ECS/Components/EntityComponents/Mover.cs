using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Zand.ECS.Components
{
    public class Mover : Component, IUpdateable
    {
        private float maxSpeed;
        public Vector2 Velocity = Vector2.Zero;

        public Mover (float maxSpeed)
        {
            this.maxSpeed = maxSpeed;
        }

        public void Update()
        {
            Entity.Position += Velocity * (float)Time.DeltaTime;
            Scene.
            UpdateEntityLayerDepth();
        }

        public void Nudge(Vector2 velocity)
        {
            Entity.Position += velocity * (float)Time.DeltaTime;
        }

        private void UpdateEntityLayerDepth()
        {
            Vector2 screenPos = Scene.Camera.GetScreenLocation(Entity.Position);
            Entity.layerDepth = MathUtil.CalculateLayerDepth(screenPos.Y, Entity.Dimensions.Y);
        }
    }
}
