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
        public Vector2 Velocity;

        public Mover (float maxSpeed)
        {
            this.maxSpeed = maxSpeed;
            Velocity = Vector2.Zero;
        }

        public void Update()
        {
            Entity.Position += Velocity * (float)Time.DeltaTime;
        }

        public void Nudge(Vector2 velocity)
        {
            Entity.Position += Vector2.Multiply(velocity, (float)Time.DeltaTime);
        }
    }
}
