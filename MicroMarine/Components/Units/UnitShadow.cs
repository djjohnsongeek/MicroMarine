using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand;

namespace MicroMarine.Components.Units
{
    class UnitShadow : Component, IRenderable
    {
        private Texture2D _shadowTexture;
        private Vector2 _entityOffset;

        public UnitShadow(Texture2D texture)
        {
            _shadowTexture = texture;
        }

        public override void OnAddedToEntity()
        {
            _entityOffset = new Vector2(10, -12);
        }

        public void Draw(SpriteBatch sbatch)
        {
            sbatch.Draw(
                _shadowTexture,
                Entity.Position,
                null,
                Color.White,
                0,
                _entityOffset,
                1,
                SpriteEffects.None,
                0.0000001f
             );
        }
    }
}
