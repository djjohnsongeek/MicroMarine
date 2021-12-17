using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand.Graphics;

namespace Zand.ECS.Components
{
    public class BoxCollider : Collider
    {
        public Rectangle HitBox;

        public BoxCollider(Rectangle hitBox)
        {
            HitBox = hitBox;
        }

        public BoxCollider(Vector2 position, int width, int height)
        {
            HitBox = new Rectangle((int)position.X, (int)position.Y, width, height);
        }

        public override void Update()
        {
            base.Update();
            HitBox.Location = Position.ToPoint();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rect = Scene.Camera.GetScreenLocation(HitBox);
            Shapes.DrawRectangle(spriteBatch, Scene.DebugPixelTexture, rect, Color.White);
        }
    }
}

