using System;
using System.Numerics;

namespace Uniswap.Client.SolidityLibs
{
    public static class FullMath
    {
        public static BigInteger MulDiv(BigInteger a, BigInteger b, BigInteger denominator)
        {
            if (denominator <= 0)
                    throw new DivideByZeroException("Denominator is zero");
                var c = a * b;
                return c / denominator;

            }

        public static BigInteger MulDivRoundingUp(BigInteger a, BigInteger b, BigInteger denominator)
        {
            var result = MulDiv(a, b, denominator);
            // ReSharper disable once InvertIf
            if (a * b % denominator > 0)
            {
                if (result >= Utils.Uint256MaxValue)
                    throw new OverflowException();
                result++;
            }

            return result;
        }
    }
}
