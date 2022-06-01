using System.Numerics;
using Nethereum.Hex.HexConvertors.Extensions;

namespace Uniswap.Client.SolidityLibs
{
    public static class FixedPoint96
    {
        public const ushort RESOLUTION = 96;
        public static BigInteger Q96 = "0x1000000000000000000000000".HexToBigInteger(false);
    }
}
