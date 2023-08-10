using System;
using System.Collections.Generic;
using System.Text;

namespace Boids
{
    static class Calc
    {
        private static Random rng = new Random();

        internal static float RandomFloat()
        {
            return (float)rng.NextDouble();
        }
    }
}
