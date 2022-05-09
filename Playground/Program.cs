using System;
using Playground.Tests;

namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            BitArrayIdentifier.Test();
            Profiler.StringIdCreationTest();
            Profiler.BitArrayIdCreationTests();
        }
    }
}
