using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand;

namespace MicroMarine.Components.Units
{
    class UnitShadow : Component, IRenderable
    {
        private Texture2D texture;
        private Vector2 _entityOffset;
        protected float _layer;

        public UnitShadow(Texture2D texture)
        {
            this.texture = texture;
            _layer = 0.0000001f;
        }

        public override void OnAddedToEntity()
        {
            _entityOffset = new Vector2(10, -12);
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
