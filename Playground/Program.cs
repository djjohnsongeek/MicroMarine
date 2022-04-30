using System;
using System.Collections.Generic;

namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            var list1 = new List<int>() { 3, 2, 8, 9 };
            var list2 = new List<int>() { 3, 8, 9, 2 };

            list1.Sort();
            list1.Sort();

            int hash1 = GetHash(list1);
            int hash2 = GetHash(list2);

            Console.WriteLine($"Hash: {GetHash(list1)}");
            Console.WriteLine($"Hash: {GetHash(list2)}");
        }

        private static int GetHash(List<int> numbers)
        {
            int hash = numbers.Count;
            foreach (var number in numbers)
            {
                hash = (hash * 31 + number);
            }

            return hash;
        }
    }
}
