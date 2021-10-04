using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zand
{
    public static class Time
    {
        public static double DeltaTime;

        public static void Update(double dt)
        {
            DeltaTime = dt;
        }

    }
}
