using Microsoft.Xna.Framework;
using Zand;
using Zand.Colliders;
using Zand.ECS.Components;

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

    }
}
