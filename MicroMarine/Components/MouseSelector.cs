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

        public MouseSelector(Rectangle hitBox, Vector2 offset) : base(hitBox, offset) { }

        public Rectangle GetScreenLocation()
        {
            return Scene.Camera.GetScreenLocation(HitBox);
        }
    }
}
