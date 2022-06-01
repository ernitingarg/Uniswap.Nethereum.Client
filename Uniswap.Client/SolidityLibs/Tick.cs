using System;
using System.Numerics;
using Nethereum.Uniswap.Contracts.UniswapV3Pool.ContractDefinition;

namespace Uniswap.Client.SolidityLibs
{
    public static class Tick
    {
        public static TicksOutputDTO Update(
            TicksOutputDTO info,
            int tick,
            int tickCurrent,
            BigInteger liquidityDelta,
            BigInteger feeGrowthGlobal0X128,
            BigInteger feeGrowthGlobal1X128,
            BigInteger secondsPerLiquidityCumulativeX128,
            long tickCumulative,
            uint time,
            bool upper,
            BigInteger maxLiquidity)
        {
            var liquidityGrossBefore = info.LiquidityGross;
            var liquidityGrossAfter = LiquidityMath.AddDelta(liquidityGrossBefore, liquidityDelta);

            if (liquidityGrossAfter > maxLiquidity)
                throw new ArgumentException("LO");

            var flipped = liquidityGrossAfter == 0 != (liquidityGrossBefore == 0);

            if (liquidityGrossBefore == 0)
            {
                // by convention, we assume that all growth before a tick was initialized happened _below_ the tick
                if (tick <= tickCurrent)
                {
                    info.FeeGrowthOutside0X128 = feeGrowthGlobal0X128;
                    info.FeeGrowthOutside1X128 = feeGrowthGlobal1X128;
                    info.SecondsPerLiquidityOutsideX128 = secondsPerLiquidityCumulativeX128;
                    info.TickCumulativeOutside = tickCumulative;
                    info.SecondsOutside = time;
                }

                info.Initialized = true;

                
            }

            info.LiquidityGross = liquidityGrossAfter;

            // when the lower (upper) tick is crossed left to right (right to left), liquidity must be added (removed)
            info.LiquidityNet = upper
                ? info.LiquidityNet - liquidityDelta
                : info.LiquidityNet + liquidityDelta;
            return info;
        }

        public static (BigInteger feeGrowthInside0X128, BigInteger feeGrowthInside1X128) GetFeeGrowthInside(
            TicksOutputDTO lower, TicksOutputDTO upper, int tickLower, int tickUpper, int tickCurrent, BigInteger feeGrowthGlobal0X128, BigInteger feeGrowthGlobal1X128)
        {
            BigInteger feeGrowthBelow0X128;
            BigInteger feeGrowthBelow1X128;
            if (tickCurrent >= tickLower)
            {
                feeGrowthBelow0X128 = lower.FeeGrowthOutside0X128;
                feeGrowthBelow1X128 = lower.FeeGrowthOutside1X128;
            }
            else
            {
                feeGrowthBelow0X128 = feeGrowthGlobal0X128 - lower.FeeGrowthOutside0X128;
                feeGrowthBelow1X128 = feeGrowthGlobal1X128 - lower.FeeGrowthOutside1X128;
            }
            
            BigInteger feeGrowthAbove0X128;
            BigInteger feeGrowthAbove1X128;

            if (tickCurrent < tickUpper)
            {
                feeGrowthAbove0X128 = upper.FeeGrowthOutside0X128;
                feeGrowthAbove1X128 = upper.FeeGrowthOutside1X128;
            }
            else
            {
                feeGrowthAbove0X128 = feeGrowthGlobal0X128 - upper.FeeGrowthOutside0X128;
                feeGrowthAbove1X128 = feeGrowthGlobal1X128 - upper.FeeGrowthOutside1X128;
            }

            var feeGrowthInside0X128 = feeGrowthGlobal0X128 - feeGrowthBelow0X128 - feeGrowthAbove0X128;
            var feeGrowthInside1X128 = feeGrowthGlobal1X128 - feeGrowthBelow1X128 - feeGrowthAbove1X128;
            return (feeGrowthInside0X128, feeGrowthInside1X128);
        }
    }
}
