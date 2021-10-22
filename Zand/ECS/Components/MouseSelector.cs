using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zand.ECS.Components
{
    class MouseSelector : Collider
    {
        
        public void Update()
        {
            Origin = Entity.Position;

            // check mouse inputs
        }
    }
}
