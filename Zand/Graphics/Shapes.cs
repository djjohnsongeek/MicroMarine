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
        public static void DrawEmptyRect(SpriteBatch sBatch, Texture2D texture, Rectangle rectangle, Color color)
        {
            DrawZeroSlopeLine(sBatch, texture, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, 1), color);
            DrawZeroSlopeLine(sBatch, texture, new Rectangle(rectangle.Right, rectangle.Top, 1, rectangle.Height), color);
            DrawZeroSlopeLine(sBatch, texture, new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, 1), color);
            DrawZeroSlopeLine(sBatch, texture, new Rectangle(rectangle.Left, rectangle.Top, 1, rectangle.Height), color);
        }

        public static void DrawZeroSlopeLine(SpriteBatch sbatch, Texture2D texture, Rectangle destRect, Color color)
        {
            sbatch.Draw(
                texture,
                destRect,
                null, // src rect
                color,
                0, // rotation
                Vector2.Zero,
                SpriteEffects.None,
                1 // ui layer depth
            );
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
                1
           );
        }

        public static void DrawRect(SpriteBatch spriteBatch, Texture2D texure, Rectangle rect, Color color)
        {
            spriteBatch.Draw(texure, rect, color);
        }
    }
}
