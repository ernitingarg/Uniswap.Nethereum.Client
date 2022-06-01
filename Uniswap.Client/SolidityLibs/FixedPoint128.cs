using System.Numerics;
using Nethereum.Hex.HexConvertors.Extensions;

namespace Uniswap.Client.SolidityLibs
{
    public static class FixedPoint128
    {
        public static BigInteger
            Q128 = "0x100000000000000000000000000000000".HexToBigInteger(false); //340282366920938463463374607431768211456
    }
}
