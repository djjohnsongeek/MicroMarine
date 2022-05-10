using System.Collections;

namespace MicroMarine.Extensions
{
    public static class BitArrayExtensions
    {
        public static bool IsEqual(this BitArray bitsA, BitArray bitsB)
        {
            if (bitsA.Length != bitsB.Length)
            {
                return false;
            }

            for (int i = 0; i < bitsA.Length; i++)
            {
                if (bitsA[i] != bitsB[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
