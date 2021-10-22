﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zand.ECS.Components
{
    public class CircleCollider : Collider
    {
        public int Radius;

        public void Update()
        {
            Origin = Entity.Position;

            // Grab near by colliders
            // check for collisions
            // resolve collisions
        }
    }
}
