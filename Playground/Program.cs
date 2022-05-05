using System;
using System.Collections;
using System.Collections.Generic;

namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] list1 = new int[] { 3, 2, 8, 9 };
            int[] list2 = new int[] { 3, 8, 9, 2 };


            var bitArray1 = new BitArray(list1);
            var bitArray2 = new BitArray(list2);


            Console.WriteLine($"Hash: {GetHash(bitArray1)}");
            Console.WriteLine($"Hash: {GetHash(bitArray2)}");
        }

        private static int GetHash(BitArray bits)
        {
            return bits.GetHashCode();
        }
    }
}
