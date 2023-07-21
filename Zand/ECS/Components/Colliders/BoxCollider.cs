using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand.Graphics;
using Zand.Physics;

namespace Zand.Colliders
{
    public class BoxCollider : Collider, ICollider, IRenderable
    {
        public Rectangle HitBox;

        public BoxCollider(Rectangle hitBox, Vector2 offset)
        {
            HitBox = hitBox;
            Offset = offset;
        }

        public override Vector2 TopLeft => new Vector2();
        public override Vector2 TopRight => new Vector2();
        public override Vector2 BottomLeft => new Vector2();
        public override Vector2 BottomRight => new Vector2();

        public override void Update()
        {
            HitBox.Location = Entity.Position.ToPoint() + Offset.ToPoint();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Debug.DebugTools.Active)
            {
                Shapes.DrawRect(spriteBatch, Entity.Scene.DebugPixelTexture, HitBox, Color.Yellow);
            }

        }
    }
}

