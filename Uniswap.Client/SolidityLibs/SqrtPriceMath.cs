using System;
using System.Numerics;

namespace Uniswap.Client.SolidityLibs
{
    public static class SqrtPriceMath
    {
        public static BigInteger GetAmount0Delta(BigInteger sqrtRatioAX96, BigInteger sqrtRatioBX96,
            BigInteger liquidity)
        {
            return
                liquidity < 0
                    ? -GetAmount0Delta(sqrtRatioAX96, sqrtRatioBX96, -liquidity, false)
                    : GetAmount0Delta(sqrtRatioAX96, sqrtRatioBX96, liquidity, true);
        }

        public static BigInteger GetAmount0Delta(BigInteger sqrtRatioAX96, BigInteger sqrtRatioBX96,
            BigInteger liquidity, bool roundUp)
        {
            if(sqrtRatioAX96 > sqrtRatioBX96)
                (sqrtRatioAX96, sqrtRatioBX96) = (sqrtRatioBX96, sqrtRatioAX96);
            var numerator1 = liquidity << FixedPoint96.RESOLUTION;
            var numerator2 = sqrtRatioBX96 - sqrtRatioAX96;
            if (sqrtRatioAX96 <= 0)
                throw new ArgumentException();
            var amount0Delta = FullMath.MulDiv(numerator1, numerator2, sqrtRatioBX96) / sqrtRatioAX96;
            
            return
                roundUp
                    ? UnsafeMath.DivRoundingUp(
                        FullMath.MulDivRoundingUp(numerator1, numerator2, sqrtRatioBX96),
                        sqrtRatioAX96)
                    : amount0Delta;
        }

        public static BigInteger GetAmount1Delta(BigInteger sqrtRatioAX96, BigInteger sqrtRatioBX96,
            BigInteger liquidity)
        {
            return
                liquidity < 0
                    ? -GetAmount1Delta(sqrtRatioAX96, sqrtRatioBX96, -liquidity, false)
                    : GetAmount1Delta(sqrtRatioAX96, sqrtRatioBX96, liquidity, true);
        }

        public static BigInteger GetAmount1Delta(BigInteger sqrtRatioAX96, BigInteger sqrtRatioBX96,
            BigInteger liquidity, bool roundUp)
        {
            if (sqrtRatioAX96 > sqrtRatioBX96)
                (sqrtRatioAX96, sqrtRatioBX96) = (sqrtRatioBX96, sqrtRatioAX96);

            return
                roundUp
                    ? FullMath.MulDivRoundingUp(liquidity, sqrtRatioBX96 - sqrtRatioAX96, FixedPoint96.Q96)
                    : FullMath.MulDiv(liquidity, sqrtRatioBX96 - sqrtRatioAX96, FixedPoint96.Q96);
        }
    }
}
