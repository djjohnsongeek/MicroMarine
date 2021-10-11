using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Zand.ECS.Components
{
    class Mover : Component, IUpdateable
    {
        private Entity _entity;
        private float _speed;
        private Vector2 _velocity;

        public Mover (Entity entity, float baseSpeed)
        {
            _speed = baseSpeed;
            _entity = entity;
            _velocity = Vector2.Zero;
        }

        public Mover (Entity entity, float baseSpeed, Vector2 startingVelocity) : this(entity, baseSpeed)
        {
            _velocity = startingVelocity;
        }

        public void Update()
        {
            _velocity.X = _velocity.X + (_speed * (float)Time.DeltaTime);
            _velocity.Y = _velocity.Y + (_speed * (float)Time.DeltaTime);

            _entity.Position = _entity.Position + _velocity;
        }
    }
}
