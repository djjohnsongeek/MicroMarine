using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zand.Assets
{
    public class SpriteSheet : Component, IRenderable
    {
        Rectangle[] _sprites;
        Texture2D _texture;

        public SpriteSheet(Texture2D texture, int spriteWidth, int spriteHeight)
        {
            _texture = texture;
            _sprites = ParseSprites(spriteWidth, spriteHeight);
        }

        private Rectangle[] ParseSprites(int spriteWidth, int spriteHeight)
        {
            // This is a pretty fragile method that expects square sprite sheets, no padding, and exact demensions
            int rowCount = _texture.Height / spriteHeight;
            int colCount = _texture.Width / spriteWidth;
            int spriteCount = rowCount * colCount;

            Rectangle[] sprites = new Rectangle[spriteCount];

            int index = 0;
            for (int y = 0; y < _texture.Height; y += spriteHeight)
            {
                for (int x = 0; x < _texture.Width; x += spriteWidth)
                {
                    sprites[index] = new Rectangle(x, y, spriteWidth, spriteHeight);
                    index++;
                }
            }

            return sprites;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Random rnd = new Random();
            int index = rnd.Next(0, 4);
            spriteBatch.Draw(_texture, Entity.Position, _sprites[index], Color.White);
        }

        
    }
}
