using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand.Graphics;
using Zand.Physics;

namespace Zand.ECS.Components
{
    public class BoxCollider : Collider, ICollider
    {
        public Rectangle HitBox;

        public BoxCollider(Rectangle hitBox, Vector2 offset)
        {
            HitBox = hitBox;
            Offset = offset;
        }

        public Vector2 TopLeft => new Vector2();
        public Vector2 TopRight => new Vector2();
        public Vector2 BottomLeft => new Vector2();
        public Vector2 BottomRight => new Vector2();

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

