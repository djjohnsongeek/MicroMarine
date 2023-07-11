using Microsoft.Xna.Framework;
using System;

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
        /// Given an Entity's Screen Y coordiante and height calculate it's layer depth.
        /// </summary>
        /// <param name="entityYPos"></param>
        /// <param name="entityHeight"></param>
        /// <returns>A float between 1.0 and 0.0 </returns>
        public static float CalculateLayerDepth(float entityYPos, int entityHeight)
        {
            return ((entityYPos + entityHeight) * 0.00001f) + 0.00001f;
        }

        public static Vector2 RandomPosition(Random rng, Vector2 origin, int maxVariation)
        {
            var x = rng.Next((int)origin.X, (int)origin.X + maxVariation);
            int y = rng.Next((int)origin.Y, (int)origin.Y + maxVariation);
            return new Vector2(x, y);
        }
    }
}
