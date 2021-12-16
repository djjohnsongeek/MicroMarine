using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand.Graphics;

namespace Zand.ECS.Components
{
    public class BoxCollider : Collider, IRenderable
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

        // Need to move this to a "primitive" draw
        public void Draw(SpriteBatch spriteBatch)
        {
            Shapes.DrawRectangle(spriteBatch, Scene.DebugPixelTexture, HitBox, Color.White);
        }
    }
}

