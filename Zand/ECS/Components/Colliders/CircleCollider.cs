using Apos.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand.Graphics;
using Zand.Physics;

namespace Zand.Colliders
{
    public class CircleCollider : Collider, ICollider
    {
        public float Radius;

        public CircleCollider(float radius, Vector2 offset) : base()
        {
            Radius = radius;
            Origin = new Vector2(radius, radius);
            Offset = offset;
        }

        // Corners
        public override Vector2 TopLeft => new Vector2(Center.X - Radius, Center.Y - Radius);
        public override Vector2 TopRight => new Vector2(Center.X + Radius, Center.Y - Radius);
        public override Vector2 BottomLeft => new Vector2(Center.X - Radius, Center.Y + Radius);
        public override Vector2 BottomRight => new Vector2(Center.X + Radius, Center.Y + Radius);

        // Edges
        public override float Right => Center.X + Radius;
        public override float Left => Center.X - Radius;
        public override float Bottom => Center.Y + Radius;
        public override float Top => Center.Y - Radius;


        public override void Draw(ShapeBatch sbatch)
        {
            sbatch.DrawCircle(Center, Radius, Color.White, Color.White);
        }
    }
}
