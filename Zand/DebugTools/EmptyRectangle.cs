using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zand.Debug
{
    public class EmptyRectangle: IRenderable
    {
        private Texture2D _texture;
        private Rectangle _rectangle;

        public EmptyRectangle()
        {

        }

        public bool Enabled { get;  }

        public void Draw(SpriteBatch sbatch)
        {

        }
    }
}
