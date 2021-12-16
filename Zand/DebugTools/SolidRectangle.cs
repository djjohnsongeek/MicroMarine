using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zand.Debug
{
    class SolidRectangle
    {
        public bool Enabled;
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

        public SolidRectangle(Texture2D texture, Point position, Point size, Color color)
        {
            _texture = texture;
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
