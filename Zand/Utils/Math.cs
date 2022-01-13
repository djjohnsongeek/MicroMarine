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
        /// <summary>
        /// Given an Entity's Screen Y coordiante and height calulate it's layer depth.
        /// </summary>
        /// <param name="entityYPos"></param>
        /// <param name="entityHeight"></param>
        /// <returns></returns>
        public static float CalculateLayerDepth(float entityYPos, int entityHeight)
        {
            return ((entityYPos + entityHeight) * 0.00001f) + 0.00001f;
        }
    }
}
