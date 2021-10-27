using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zand.ECS.Components
{
    public class BoxCollider : Collider
    {
        public float Width { get; private set; }
        public float Height { get; private set; }


        public void Update()
        {
            Origin = Entity.Position;

            // find nearby collision objects
            // check for collisions & resolve
        }
    }
}
