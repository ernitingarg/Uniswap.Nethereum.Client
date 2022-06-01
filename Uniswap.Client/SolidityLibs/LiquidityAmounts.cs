using System.Numerics;

namespace Uniswap.Client.SolidityLibs
{
    public static class LiquidityAmounts
    {
        public static BigInteger GetLiquidityForAmount0(BigInteger sqrtRatioAX96,
            BigInteger sqrtRatioBX96,
            BigInteger amount0)
        {
            if (sqrtRatioAX96 > sqrtRatioBX96) (sqrtRatioAX96, sqrtRatioBX96) = (sqrtRatioBX96, sqrtRatioAX96);
            var intermediate = FullMath.MulDiv(sqrtRatioAX96, sqrtRatioBX96, FixedPoint96.Q96);
            return FullMath.MulDiv(amount0, intermediate, sqrtRatioBX96 - sqrtRatioAX96);
        }

        public static BigInteger GetLiquidityForAmount1(BigInteger sqrtRatioAX96,
            BigInteger sqrtRatioBX96,
            BigInteger amount1)
        {
            if (sqrtRatioAX96 > sqrtRatioBX96) (sqrtRatioAX96, sqrtRatioBX96) = (sqrtRatioBX96, sqrtRatioAX96);
            return FullMath.MulDiv(amount1, FixedPoint96.Q96, sqrtRatioBX96 - sqrtRatioAX96);
        }

        public static BigInteger GetLiquidityForAmounts(BigInteger sqrtRatioX96,
            BigInteger sqrtRatioAX96,
            BigInteger sqrtRatioBX96,
            BigInteger amount0,
            BigInteger amount1)
        {
            BigInteger liquidity = 0;
            if (sqrtRatioAX96 > sqrtRatioBX96) (sqrtRatioAX96, sqrtRatioBX96) = (sqrtRatioBX96, sqrtRatioAX96);
            if (sqrtRatioX96 <= sqrtRatioAX96)
            {
                liquidity = GetLiquidityForAmount0(sqrtRatioAX96, sqrtRatioBX96, amount0);
            }
            else if (sqrtRatioX96 < sqrtRatioBX96)
            {
                var liquidity0 = GetLiquidityForAmount0(sqrtRatioX96, sqrtRatioBX96, amount0);
                var liquidity1 = GetLiquidityForAmount1(sqrtRatioAX96, sqrtRatioX96, amount1);

                liquidity = liquidity0 < liquidity1 ? liquidity0 : liquidity1;
            }
            else
            {
                liquidity = GetLiquidityForAmount1(sqrtRatioAX96, sqrtRatioBX96, amount1);
            }

            return liquidity;
        }

        public static (BigInteger, BigInteger) GetAmountsForLiquidity(BigInteger sqrtRatioX96,
            BigInteger sqrtRatioAX96,
            BigInteger sqrtRatioBX96,
            BigInteger liquidity)
        {
            BigInteger amount0 = 0;
            BigInteger amount1 = 0;

            if (sqrtRatioAX96 > sqrtRatioBX96) (sqrtRatioAX96, sqrtRatioBX96) = (sqrtRatioBX96, sqrtRatioAX96);
            if (sqrtRatioX96 <= sqrtRatioAX96)
            {
                amount0 = GetAmount0ForLiquidity(sqrtRatioAX96, sqrtRatioBX96, liquidity);
            }
            else if (sqrtRatioX96 < sqrtRatioBX96)
            {
                amount0 = GetAmount0ForLiquidity(sqrtRatioX96, sqrtRatioBX96, liquidity);
                amount1 = GetAmount1ForLiquidity(sqrtRatioAX96, sqrtRatioX96, liquidity);
            }
            else
            {
                amount1 = GetAmount1ForLiquidity(sqrtRatioAX96, sqrtRatioBX96, liquidity);
            }

            return (amount0, amount1);
        }


        public static BigInteger GetAmount0ForLiquidity(BigInteger sqrtRatioAX96, BigInteger sqrtRatioBX96,
            BigInteger liquidity)
        {
            if (sqrtRatioAX96 > sqrtRatioBX96) (sqrtRatioAX96, sqrtRatioBX96) = (sqrtRatioBX96, sqrtRatioAX96);
            return
                FullMath.MulDiv(
                    liquidity << FixedPoint96.RESOLUTION,
                    sqrtRatioBX96 - sqrtRatioAX96,
                    sqrtRatioBX96
                ) / sqrtRatioAX96;
        }

        public static BigInteger GetAmount1ForLiquidity(BigInteger sqrtRatioAX96, BigInteger sqrtRatioBX96,
            BigInteger liquidity)
        {
            if (sqrtRatioAX96 > sqrtRatioBX96) (sqrtRatioAX96, sqrtRatioBX96) = (sqrtRatioBX96, sqrtRatioAX96);
            return FullMath.MulDiv(liquidity, sqrtRatioBX96 - sqrtRatioAX96, FixedPoint96.Q96);
        }
    }
}
