using Microsoft.Xna.Framework.Graphics;

namespace Zand.UI
{
    public abstract class UIElement : IUpdateable, IRenderable
    {
        protected bool _enabled;
        protected Scene _scene;

        public bool Enabled => _enabled;


        public abstract void Update();
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
