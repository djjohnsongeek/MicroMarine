using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zand.Components
{
    public class SimpleSprite : Component, IRenderable
    {
        private Texture2D _texture;

        public SimpleSprite(Texture2D texture)
        {
            _texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Entity.Position, Color.White);
        }
    }
}
