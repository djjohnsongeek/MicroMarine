using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand.Graphics;

namespace Zand.ECS.Components
{
    public class BoxCollider : Collider
    {
        public Rectangle HitBox;

        public BoxCollider(Rectangle hitBox, Vector2 offset)
        {
            HitBox = hitBox;
            Offset = offset;
        }

        public BoxCollider(Vector2 position, int width, int height)
        {
            HitBox = new Rectangle((int)position.X, (int)position.Y, width, height);
        }

        public override void Update()
        {
            HitBox.Location = Entity.Position.ToPoint() + Offset.ToPoint();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rect = Scene.Camera.GetScreenLocation(HitBox);
            Shapes.DrawRect(spriteBatch, Scene.DebugPixelTexture, rect, new Color(180, 255, 0, 175));
        }
    }
}

