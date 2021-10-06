using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zand.ECS.Components
{
    public class SpriteComponent : Component, IRenderable
    {
        private Texture2D _texture;
        private Vector2 _position;

        public SpriteComponent(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            _position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White); 
        }
    }
}
