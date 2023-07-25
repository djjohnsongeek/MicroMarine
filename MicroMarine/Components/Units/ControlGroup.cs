using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand;

namespace MicroMarine.Components
{
    class ControlGroup : RenderableComponent
    {
        public byte Id { get; private set; }

        public ControlGroup()
        {
            Id = 0;
        }

        public void SetControlGroup(byte id)
        {
            Id = id;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Id == 0) return;

            var font = Scene.Content.GetContent<SpriteFont>("DebugFont");
            spriteBatch.DrawString(
                font,
                Id.ToString(),
                Entity.Position,
                Color.White,
                0,
                new Vector2(-12, -8),
                Vector2.One,
                SpriteEffects.None,
                0);
        }
    }
}
