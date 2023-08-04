using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zand.ECS.Components
{
    public class BouncingSprite : RenderableComponent, IUpdateable
    {
        private Texture2D _texture;
        private Texture2D _shadowTexture;
        private float _lifetime;
        public Vector2 Velocity;
        public float Z;
        public float Zspeed = 0;
        private float _gravity = -1f;
        private float _groundFriction = -.6f;
        private float _elasticity = .6f;
        private float _rotation = 0f;
        private float _rotationSpeed = 5f;

        public BouncingSprite(Vector3 startingVelocity, float startingHeight, Texture2D texture, Texture2D shadow, float lifetime = 30)
        {
            Z = startingHeight;
            Zspeed = startingVelocity.Z;
            _texture = texture;
            _shadowTexture = shadow;
            Velocity = new Vector2(startingVelocity.X, startingVelocity.Y);
            _lifetime = lifetime;
        }

        public override void OnAddedToEntity()
        {
            _rotationSpeed = (float)Entity.Scene.Rng.Next(5, 10);
            Time.AddTimer(_lifetime, Entity.Destroy);
        }

        public override void OnRemovedFromEntity()
        {
            _texture = null;
            _shadowTexture = null;
        }

        public void Update()
        {
            if (Z <= 0)
            {
                Velocity += Velocity * _groundFriction;
            }

            Entity.Position += Velocity * (float)Time.DeltaTime;
            _rotation += _rotationSpeed * (float)Time.DeltaTime;

            RenderDepth = Calc.CalculateRenderDepth(Entity.Scene.Camera.GetScreenLocation(Entity.Position).Y, _texture.Height);

            Zspeed += _gravity * (float)Time.DeltaTime;
            Z += Zspeed;

            if (Z <= 0)
            {
                Zspeed = -Zspeed * _elasticity;
                _rotationSpeed = _rotationSpeed * _elasticity;
                Z = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _texture,
                new Vector2(Entity.Position.X, Entity.Position.Y - Z),
                null,
                Color.White,
                _rotation,
                new Vector2(_texture.Width / 2, _texture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0 // no longer used
            );


            if (true)
            {
                spriteBatch.Draw(
                    _shadowTexture,
                    Entity.Position,
                    null,
                    Color.White,
                    0,
                    new Vector2(_shadowTexture.Width / 2, _shadowTexture.Height - 5),
                    CalculateShadowScale(),
                    SpriteEffects.None,
                    0 // no longer used
                );
            }
        }

        private Vector2 CalculateShadowScale()
        {
            float scale = 1 / (1 + Z / 20);
            return new Vector2(scale, scale);
        }
    }
}
