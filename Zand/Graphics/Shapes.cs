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

        public static void DrawLine(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 end, int width, Color color)
        {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);

            spriteBatch.Draw(
                texture,
                new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), width),
                null,
                color,
                angle,
                new Vector2(0, 0),
                SpriteEffects.None,
                0
           );
        }
    }
}
