using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Zand.Graphics
{
    public static class Shapes
    {
        private static Dictionary<int, Texture2D> _circleTextures = new Dictionary<int, Texture2D>();

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

        public static void DrawLine(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 end, int width, Color color, float depth = 1)
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
                depth
           );
        }

        public static void DrawRect(SpriteBatch spriteBatch, Texture2D texure, Rectangle rect, Color color)
        {
            spriteBatch.Draw(texure, rect, color);
        }

        public static Texture2D CreateCircleTexture(float diameter)
        {
            return CreateCircleTexture((int)diameter);
        }

        public static Texture2D CreateCircleTexture(int diameter)
        {
            if (Shapes._circleTextures.ContainsKey(diameter))
            {
                return _circleTextures[diameter];
            }


            Texture2D texture = new Texture2D(Core._instance.GraphicsDevice, diameter, diameter);
            Color[] colorData = new Color[diameter * diameter];

            float radius = diameter / 2f;
            float radiusSq = radius * radius;

            for (int x = 0; x < diameter; x++)
            {
                for (int y = 0; y < diameter; y++)
                {
                    int index = x * diameter + y;
                    Vector2 pos = new Vector2(x - radius, y - radius);
                    if (pos.LengthSquared() <= radiusSq)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);

            Shapes._circleTextures.Add(diameter, texture);

            return texture;
        }

        
    }
}
