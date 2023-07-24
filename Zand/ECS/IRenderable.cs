using Microsoft.Xna.Framework.Graphics;

namespace Zand
{
    public interface IRenderable
    {
        public bool Enabled { get; }
        public float RenderDepth { get; set; }
        public int RenderLayer { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
