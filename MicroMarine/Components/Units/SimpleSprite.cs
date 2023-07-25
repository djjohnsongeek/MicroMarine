using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand;

namespace MicroMarine.Components.Units
{
    class SimpleSprite : RenderableComponent
    {
        protected Texture2D texture;
        protected Vector2 _entityOffset;

        public SimpleSprite(Texture2D texture, Vector2 entityOffset)
        {
            this.texture = texture;
            _entityOffset = entityOffset;
        }

        public override void Draw(SpriteBatch sbatch)
        {
            sbatch.Draw(
                texture,
                Entity.Position,
                null,
                Color.White,
                0,
                _entityOffset,
                1,
                SpriteEffects.None,
                0 // ignored now
             );
        }
    }
}
