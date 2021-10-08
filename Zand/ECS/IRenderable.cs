using Microsoft.Xna.Framework.Graphics;

namespace Zand
{
    internal interface IRenderable
    {
        bool Enabled { get; }
        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
