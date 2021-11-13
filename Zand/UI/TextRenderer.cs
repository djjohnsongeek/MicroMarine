using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Zand.UI
{
    public class TextRenderer
    {
        private SpriteFont smallFont;

        public TextRenderer(SpriteFont font)
        {
            smallFont = font;
        }

        public void DrawString(SpriteBatch spriteBatch, string text, Vector2 position, Color color, float scale, bool center)
        {
            if (center)
            {
                Vector2 stringSize = smallFont.MeasureString(text) * 0.5f;
                spriteBatch.DrawString(
                    smallFont,
                    text,
                    position - (stringSize * scale),
                    color,
                    0.0f,
                    Vector2.Zero,
                    scale,
                    SpriteEffects.None,
                    0.0f
                );
            }
            else
            {
                spriteBatch.DrawString(
                    smallFont,
                    text,
                    position,
                    color,
                    0.0f,
                    Vector2.Zero,
                    scale,
                    SpriteEffects.None,
                    0.0f
               );
            }
        }
    }
}
