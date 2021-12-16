using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zand.Graphics
{
    public static class Shapes
    {
        public static void DrawRectangle(SpriteBatch sBatch, Texture2D texture, Rectangle rectangle, Color color)
        {
            sBatch.Draw(texture, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, 1), color);
            sBatch.Draw(texture, new Rectangle(rectangle.Right, rectangle.Top, 1, rectangle.Height), color);
            sBatch.Draw(texture, new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, 1), color);
            sBatch.Draw(texture, new Rectangle(rectangle.Left, rectangle.Top, 1, rectangle.Height), color);
        }
    }
}
