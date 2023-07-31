using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand.Graphics;
using Zand.Physics;

namespace Zand.Colliders
{
    public class BoxCollider : Collider, ICollider
    {
        public Rectangle HitBox;

        public BoxCollider(Rectangle hitBox, Vector2 offset)
        {
            HitBox = hitBox;
            Offset = offset;
        }

        public new Vector2 Center => new Vector2(HitBox.Left + HitBox.Width / 2, HitBox.Top + HitBox.Height / 2);
        public override Vector2 TopLeft => new Vector2(HitBox.Left, HitBox.Top);
        public override Vector2 TopRight => new Vector2(HitBox.Right, HitBox.Top);
        public override Vector2 BottomLeft => new Vector2(HitBox.Left, HitBox.Bottom);
        public override Vector2 BottomRight => new Vector2(HitBox.Right, HitBox.Bottom);

        public override void Draw(SpriteBatch spriteBatch)
        {
            Shapes.DrawRect(spriteBatch, Entity.Scene.DebugPixelTexture, HitBox, Tint);
        }
    }
}

