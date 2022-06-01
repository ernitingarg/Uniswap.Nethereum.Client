using System;
using System.Numerics;
using Nethereum.Uniswap.Contracts.UniswapV3Pool.ContractDefinition;

namespace Uniswap.Client.SolidityLibs
{
    public static class Position
    {
        public static PositionsOutputDTO Update(PositionsOutputDTO self, BigInteger liquidityDelta,
            BigInteger feeGrowthInside0X128, BigInteger feeGrowthInside1X128)
        {
            var _self = self;
            BigInteger liquidityNext;
            if (liquidityDelta == 0)
            {
                if (_self.Liquidity <= 0)
                    throw new ArgumentException("NP");
                liquidityNext = _self.Liquidity;
            }
            else
            {
                liquidityNext = LiquidityMath.AddDelta(_self.Liquidity, liquidityDelta);
            }
            // calculate accumulated fees
            var deltafee0 = feeGrowthInside0X128 - _self.FeeGrowthInside0LastX128;
            var deltafee1 = feeGrowthInside1X128 - _self.FeeGrowthInside1LastX128;
            if (deltafee0 < 0)
            {
                deltafee0 = Utils.Uint256MaxValue + feeGrowthInside0X128 - _self.FeeGrowthInside0LastX128 + 1;
            }
            if (deltafee1 < 0)
            {
                deltafee0 = Utils.Uint256MaxValue + feeGrowthInside1X128 - _self.FeeGrowthInside1LastX128 + 1;
            }
            var tokensOwed0 = FullMath.MulDiv(
                deltafee0,
                _self.Liquidity,
                FixedPoint128.Q128);
            var tokensOwed1 = FullMath.MulDiv(
                deltafee1,
                _self.Liquidity,
                FixedPoint128.Q128);
            if (liquidityDelta != 0) self.Liquidity = liquidityNext;
            self.FeeGrowthInside0LastX128 = feeGrowthInside0X128;
            self.FeeGrowthInside1LastX128 = feeGrowthInside1X128;
            if (tokensOwed0 <= 0 && tokensOwed1 <= 0) return self;
            // overflow is acceptable, have to withdraw before you hit type(uint128).max fees
            self.TokensOwed0 += tokensOwed0;
            self.TokensOwed1 += tokensOwed1;
            return self;
        }
    }
}
