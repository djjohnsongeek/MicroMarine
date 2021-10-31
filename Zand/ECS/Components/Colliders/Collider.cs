using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zand.ECS.Components
{
    public class Collider : Component, IUpdateable
    {
        public Vector2 Position;

        public void Update()
        {
            Position = Entity.Position;
        }

    }
}
