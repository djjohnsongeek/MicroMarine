using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zand.Assets
{
    public class SpriteSheet : Component
    {
        Rectangle[] _sprites;
        Texture2D _texture;

        public SpriteSheet(Texture2D texture, int spriteWidth, int spriteHeight)
        {
            _texture = texture;
            _sprites = ParseSprites(spriteWidth, spriteHeight);
        }

        public Rectangle this[int index]
        {
            get => _sprites[index];
        }

        public Rectangle[] GetFrames(int start, int end)
        {
            if (end < start)
            {
                throw new ArgumentException("The 'start' parameter cannot be larger then 'end' parameter", "start");
            }

            int size = end - start + 1;
            Rectangle[] frames = new Rectangle[size];

            int i = 0;
            for (int spriteIndex = start; spriteIndex <= end; spriteIndex++)
            {
                frames[i] = _sprites[spriteIndex];
                i++;
            }

            return frames;
        }

        public Rectangle GetFrame(int index)
        {
            return _sprites[index];
        }

        public Texture2D Texture => _texture;

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

        public override void OnRemovedFromEntity()
        {
            Array.Clear(_sprites, 0, _sprites.Length);
            //_texture.Dispose();
            base.OnRemovedFromEntity();
        }
    }
}
