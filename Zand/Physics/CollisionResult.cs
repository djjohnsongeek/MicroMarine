using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zand.Physics
{
    public struct CollisionResult
    {
        public bool Collides;
        public float Distance;
        public float SafeDistance;
        public float RepelStrength;
        public float Angle;

        public void SetRepelStrength()
        {
            RepelStrength = SafeDistance + (Distance / SafeDistance);
        }
    }
}
