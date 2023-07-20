using Microsoft.Xna.Framework.Graphics;

namespace Zand
{
    public interface IRenderable
    {
        public bool Enabled { get; }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
