using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground.Tests
{
    public static class BitArrayIdentifier
    {
        public static int[] EntityIds = new int[] { 1, 4, 8, 14, 20 };

        public static void Test()
        {
            BitArray bits = new BitArray(21, false);


            foreach (var id in EntityIds)
            {
                bits[id] = true;
            }
            
            foreach (var id in EntityIds)
            {
                Console.WriteLine(bits[id]);
            }
            Console.WriteLine(bits[17]);



        }
    }
}
