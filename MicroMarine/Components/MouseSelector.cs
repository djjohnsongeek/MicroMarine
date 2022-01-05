using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand;
using Zand.ECS.Components;
using Zand.Physics;

namespace MicroMarine.Components
{
    public class MouseSelector : BoxCollider, Zand.IUpdateable
    {
        public bool Selected = false;

        public MouseSelector(Rectangle hitBox) : base(hitBox) { }

        public new void Update()
        {
            Rectangle rect = Scene.Camera.GetScreenLocation(HitBox);

            if (Collisions.RectangleToPoint(rect, Input.MousePosition) && Input.LeftMouseWasPressed())
            {
                Selected = !Selected;
                Entity.GetComponent<Health>().Visible = Selected;
            }
        }
    }
}
