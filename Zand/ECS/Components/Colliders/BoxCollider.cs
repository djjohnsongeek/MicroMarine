using Apos.Shapes;
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

        public override float Top => HitBox.Top;
        public override float Left => HitBox.Left;
        public override float Bottom => HitBox.Bottom;
        public override float Right => HitBox.Right;

        public override Vector2 TopLeft => new Vector2(Left, Top);
        public override Vector2 TopRight => new Vector2(Right, Top);
        public override Vector2 BottomLeft => new Vector2(Left, Bottom);
        public override Vector2 BottomRight => new Vector2(Right, Bottom);

        public override void Draw(ShapeBatch shapeBatch)
        {
            shapeBatch.DrawRectangle(HitBox.Location.ToVector2(), HitBox.Size.ToVector2(), InCollision ? Color.Red : Color.White, Color.Black);
        }
    }
}

