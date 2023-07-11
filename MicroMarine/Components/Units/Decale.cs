using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand;

namespace MicroMarine.Components.Units
{
    class Decale : Component, IRenderable
    {
        private Texture2D texture;
        private Vector2 _entityOffset;
        protected float _layer;

        public Decale(Texture2D texture, Vector2 entityOffset)
        {
            this.texture = texture;
            _layer = 0.0000001f;
            _entityOffset = entityOffset;
        }

        public virtual void Draw(SpriteBatch sbatch)
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
                _layer
             );
        }
    }
}
