using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand.Physics;

namespace Zand.ECS.Components
{
    public class MouseSelector : BoxCollider, IUpdateable
    {
        public bool Selected = false;

        public new void Update()
        {
            base.Update();

            Rectangle rect = Scene.Camera.GetScreenLocation(HitBox);

            if (Collisions.RectangleToPoint(rect, Input.MousePosition) && Input.LeftMouseWasPressed())
            {
                Selected = true;
                Scene.DebugTools.Log("Box collider clicked");
            }
        }

        public MouseSelector(Rectangle hitBox) : base(hitBox) { }

    }
}
