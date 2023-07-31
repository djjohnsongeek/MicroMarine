using Microsoft.Xna.Framework;
using Zand.Colliders;

namespace Zand.Components
{
    public class MouseSelectCollider : BoxCollider, IUpdateable
    {
        public bool Selected = false;

        public MouseSelectCollider(Rectangle hitBox, Vector2 offset) : base(hitBox, offset) { }

        public Rectangle GetScreenLocation()
        {
            return Scene.Camera.GetScreenLocation(HitBox);
        }

        public override void Update()
        {
            HitBox.Location = Entity.Position.ToPoint() + Offset.ToPoint();
        }
    }
}
