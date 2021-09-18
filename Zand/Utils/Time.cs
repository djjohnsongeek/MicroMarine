using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zand
{
    public static class Time
    {
        public static double deltaTime;

        public static void Update(double dt)
        {
            deltaTime = dt;
        }

    }
}
