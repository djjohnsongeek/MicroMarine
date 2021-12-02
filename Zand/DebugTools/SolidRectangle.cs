using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zand.Debug
{
    class SolidRectangle
    {
        private Texture2D _texture;
        private Color _color;
        private Rectangle _rectangle;

        public Vector2 Position
        {
            get
            {
                return new Vector2(_rectangle.X, _rectangle.Y);
            }
        }

        public SolidRectangle(GraphicsDevice graphics, Point position, Point size, Color color)
        {
            _texture = new Texture2D(graphics, 1, 1);
            _texture.SetData(new Color[] { color });
            _rectangle = new Rectangle(position, size);
            _color = color;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _texture,
                new Vector2(_rectangle.X, _rectangle.Y),
                _rectangle,
                _color
            );
        }
    }
}
