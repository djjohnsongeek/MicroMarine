using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zand;
using Zand.Colliders;
using Zand.ECS.Components;
using Zand.Graphics;

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
