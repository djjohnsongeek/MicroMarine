using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroMarine.Extensions;

namespace Playground.Tests
{
    public static class BitArrayIdentifier
    {
        public static int[] EntityIdsA = new int[] { 1, 4, 8, 14, 20 };
        public static int[] EntityIdsB = new int[] { 1, 4, 8, 14, 20 };

        public static void Test()
        {
            BitArray bitsA = new BitArray(21, false);
            BitArray bitsB = new BitArray(21, false);


            foreach (var id in EntityIdsA)
            {
                bitsA[id] = true;
            }

            foreach (var id in EntityIdsB)
            {
                bitsB[id] = true;
            }

            Console.WriteLine($"BitArrays are the same? {bitsA.IsEqual(bitsB)}");
        }
    }
}
