using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand.Physics;

namespace Zand.ECS.Components
{
    public class MouseSelector : BoxCollider
    {
        public bool Selected = false;

        public new void Update()
        {
            base.Update();

            

            if (Collisions.RectangleToPoint(HitBox, Scene.Camera.GetWorldLocation(Input.MousePosition)) && Input.LeftMouseWasPressed())
            {
                Selected = true;
            }
        }

        public MouseSelector(Rectangle hitBox) : base(hitBox) { }

    }
}
