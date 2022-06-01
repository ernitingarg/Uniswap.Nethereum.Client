using System;
using System.Numerics;

namespace Uniswap.Client.SolidityLibs
{
    public static class LiquidityMath
    {
        public static BigInteger AddDelta(BigInteger x, BigInteger y)
        {
            if (y < 0)
            {
                var z = x - (-y);
                if (z >= x)
                    throw new ArgumentOutOfRangeException("LS", innerException: null);
                return z;
            }
            else
            {
                var z = x + y;
                if (z < x)
                    throw new ArgumentOutOfRangeException("LA", innerException: null);
                return z;
            }
        }
    }
}
