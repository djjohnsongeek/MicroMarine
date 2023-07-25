using Microsoft.Xna.Framework.Graphics;
using Zand;
using Apos.Tweens;
using Microsoft.Xna.Framework;

namespace MicroMarine.Components
{
    class DropShip : RenderableComponent, Zand.IUpdateable
    {

        FloatTween Z;
        private Texture2D _texture;
        private Vector2 _origin;


        public DropShip(float startHeight)
        {
            Z = new FloatTween(startHeight, 0, 1500, Easing.CubeOut);
        }


        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            _texture = Entity.Scene.Content.GetContent<Texture2D>("dropShip");
            _origin = new Vector2(_texture.Width * .5f, _texture.Height);
        }

        public override void OnRemovedFromEntity()
        {
            base.OnRemovedFromEntity();
        }

        public void Update()
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _texture,
                Entity.Position - new Vector2(0, Z.Value),
                null,
                Color.White,
                0,
                _origin,
                Vector2.One,
                SpriteEffects.None,
                0 // ignored now
           );
        }
    }
}
