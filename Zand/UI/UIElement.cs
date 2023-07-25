using Microsoft.Xna.Framework.Graphics;

namespace Zand.UI
{
    public abstract class UIElement
    {
        protected bool _enabled;
        protected Scene _scene;

        public bool Enabled => throw new System.NotImplementedException();

        public abstract void Update();

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
