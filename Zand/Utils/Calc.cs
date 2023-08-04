using Microsoft.Xna.Framework;
using System;

namespace Zand
{
    public static class Calc
    {
        public static Random Rng = new Random();

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


        public static float ClampFloat(float min, float max, float value)
        {
            if (value < min)
            {
                return min;
            }

            if (value > max)
            {
                return max;
            }

            return value;
        }

        public static int CompareFloat(float x, float y)
        {
            if (x == y)
            {
                return 0;
            }

            return (int)(x - y);
        }


        /// <summary>
        /// Given an Entity's Screen Y coordiante and height calculate it's layer depth.
        /// </summary>
        /// <param name="entityYPos"></param>
        /// <param name="entityHeight"></param>
        /// <returns>A float between 1.0 and 0.0 </returns>
        public static float CalculateRenderDepth(float entityYPos, int entityHeight)
        {
            return ((entityYPos) * 0.00001f) + 0.00001f;
        }

        public static Vector2 RandomPosition(Random rng, Vector2 origin, int maxVariation)
        {
            var x = rng.Next((int)origin.X, (int)origin.X + maxVariation);
            int y = rng.Next((int)origin.Y, (int)origin.Y + maxVariation);
            return new Vector2(x, y);
        }

        public static int Random(int min, int max)
        {
            return Rng.Next(min, max + 1);
        }

        public static Vector2 AbsVector2(Vector2 v)
        {
            v.X = Math.Abs(v.X);
            v.Y = Math.Abs(v.Y);
            return v;
        }
    }
}
