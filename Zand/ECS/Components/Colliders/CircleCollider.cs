using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand.Physics;

namespace Zand.Colliders
{
    public class CircleCollider : Collider, ICollider
    {
        public float Radius;
        private Texture2D _texture;

        /// <summary>
        ///  Requires a 'Circle' Texture
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="radius"></param>
        public CircleCollider(Texture2D texture, float radius, Vector2 offset) : base()
        {
            Radius = radius;
            _texture = texture;
            Origin = new Vector2(radius, radius);
            Offset = offset;
        }

        // Corners
        public override Vector2 TopLeft => new Vector2(Center.X - Radius, Center.Y - Radius);
        public override Vector2 TopRight => new Vector2(Center.X + Radius, Center.Y - Radius);
        public override Vector2 BottomLeft => new Vector2(Center.X - Radius, Center.Y + Radius);
        public override Vector2 BottomRight => new Vector2(Center.X + Radius, Center.Y + Radius);

        // Center Edges
        public override Vector2 TopCenter => new Vector2(Center.X, Top);
        public override Vector2 RightCenter => new Vector2(Right, Center.Y);
        public override Vector2 BottomCenter => new Vector2(Center.X, Bottom);
        public override Vector2 LeftCenter => new Vector2(Left, Center.Y);


        // Edges
        public override float Right => Center.X + Radius;
        public override float Left => Center.X - Radius;
        public override float Bottom => Center.Y + Radius;
        public override float Top => Center.Y - Radius;


        public override void Draw(SpriteBatch sbatch)
        {
            sbatch.Draw(_texture, Entity.Scene.Camera.GetScreenLocation(Center), null, Tint, 0, Origin, 1, SpriteEffects.None, 0);
        }
    }
}
