using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zand.ECS.Components
{
    public class BouncingSprite : Component, IUpdateable, IRenderable
    {
        private Texture2D _texture;
        private Texture2D _shadowTexture;
        private float _lifetime;
        public Vector2 Velocity;
        public float Z;
        public float gravity = .5f;
        public float bounce = 60;


        public BouncingSprite(Vector2 startingVelocity, float startingHeight, Texture2D texture, Texture2D shadow)
        {
            _texture = texture;
            _shadowTexture = shadow;
            Velocity = startingVelocity;
            Z = 20;
        }


        public void Update()
        {
            Entity.Position += Velocity * (float)Time.DeltaTime *.8f;
            Entity.layerDepth = MathUtil.CalculateLayerDepth(Entity.ScreenPosition.Y, _texture.Height);


            Z -= gravity;
            if (Z < 0)
            {
                Z += bounce;
                bounce *= .8f;

                if (bounce < 0)
                {
                    gravity = 0;
                    Z = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture,
                new Vector2(Entity.Position.X, Entity.Position.Y - Z),
                null,
                Color.White,
                1,
                new Vector2(_texture.Width / 2, _texture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                Entity.layerDepth);


            spriteBatch.Draw(_shadowTexture,
                Entity.Position,
                null,
                Color.White,
                0,
                new Vector2(_shadowTexture.Width / 2, _shadowTexture.Width / 2),
                Vector2.One,
                SpriteEffects.None,
                Entity.layerDepth);
        }
    }
}
