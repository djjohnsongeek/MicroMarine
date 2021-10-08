using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zand.ECS.Components
{
    public class SimpleSprite : Component, IRenderable
    {
        private Texture2D _texture;
        private Entity _entity;

        public SimpleSprite(Texture2D texture, Entity entity)
        {
            _texture = texture;
            _entity = entity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _entity.Position, Color.White); 
        }
    }
}
