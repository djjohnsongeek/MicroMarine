using System;

namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] list1 = new int[] { 3, 7, 8, 6 };
            int[] list2 = new int[] { 2, 8, 8, 5 };

            Console.WriteLine($"Hash: {GetHash(list1)}");
            Console.WriteLine($"Hash: {GetHash(list2)}");
        }

        private static int GetHash(int[] nums)
        {
            int n = nums[0];
            for (int i = 0; i < nums.Length; i++)
            {
                n -= nums[i];
            }
            return n;
        }
    }
}
