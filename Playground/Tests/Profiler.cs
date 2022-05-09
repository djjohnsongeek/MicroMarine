using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground.Tests
{
    public static class Profiler
    {
        private static int testCycles = 50000;
        private static int unitsPerGroup = 1000;
        public static void StringIdCreationTest()
        {
            // Initialize
            int[][] testCases = InitTestCases();
            var timer = new Stopwatch();

            StringBuilder sBuilder = new StringBuilder();
            timer.Start();
            for (int i = 0; i < testCases.Length; i++)
            {
                GetStringId(testCases[i], sBuilder);
            }
            timer.Stop();
            Console.WriteLine($"{testCycles - 1} String Ids created in {timer.ElapsedMilliseconds} milliseconds.");
        }

        public static void BitArrayIdCreationTests()
        {
            int[][] testCases = InitTestCases();
            var timer = new Stopwatch();

            timer.Start();
            for (int i = 0; i < testCases.Length; i ++)
            {
                GetBitArrayId(testCases[i]);
            }
            timer.Stop();
            Console.WriteLine($"{testCycles - 1} BitArray Ids created in {timer.ElapsedMilliseconds} milliseconds.");

        }

        private static int[][] InitTestCases()
        {
            Console.WriteLine("Initializing ...");
            int[][] testCases = new int[testCycles][];
            var rand = new Random();
            for (int i = 0; i < testCycles; i++)
            {
                testCases[i] = new int[unitsPerGroup];
                for (int j = 0; j < unitsPerGroup; j++)
                {
                    testCases[i][j] = rand.Next(1, unitsPerGroup - 1);
                }
            }

            Console.WriteLine("Initializing Complete.");
            return testCases;
        }

        private static string GetStringId(int[] unitIds, StringBuilder builder)
        {
            builder.Clear();
            for (int i = 0; i < unitIds.Length; i++)
            {
                builder.Append(unitIds[i].ToString());
            }

            return builder.ToString();
        }

        private static BitArray GetBitArrayId(int[] unitsIds)
        {
            return new BitArray(unitsIds);
        }
    }
}
