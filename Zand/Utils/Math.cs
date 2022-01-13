using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zand
{
    public static class MathUtil
    {
        public static float ClampFloat(float x)
        {
            if (x > 1f)
            {
                return 1f;
            }

            if (x < 0f)
            {
                return 0f;
            }

            return x;
        }
    }
}
