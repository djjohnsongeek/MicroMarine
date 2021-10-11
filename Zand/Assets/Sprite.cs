using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zand.Assets
{
    public class Sprite
    {
        private Texture2D _texture;
        private Rectangle _soureRect;


        Sprite(Texture2D texture, Rectangle sourceRect)
        {
            _texture = texture;
            _soureRect = sourceRect;
        }
}
}
