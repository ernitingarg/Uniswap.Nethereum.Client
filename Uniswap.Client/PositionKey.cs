using Nethereum.ABI;
using Nethereum.Hex.HexConvertors.Extensions;

namespace Uniswap.Client
{
    public static class PositionKey
    {
        public static string ComputeHex(string address, int tickLower, int tickUpper)
        {
            var values = new ABIValue[3];
            values[0] = new ABIValue("address", address);
            values[1] = new ABIValue("int24", tickLower);
            values[2] = new ABIValue("int24", tickUpper);
            var encoder = new ABIEncode();

            return encoder.GetSha3ABIEncodedPacked(values).ToHex();
        }

        public static byte[] Compute(string address, int tickLower, int tickUpper)
        {
            var values = new ABIValue[3];
            values[0] = new ABIValue("address", address);
            values[1] = new ABIValue("int24", tickLower);
            values[2] = new ABIValue("int24", tickUpper);
            var encoder = new ABIEncode();
            return encoder.GetSha3ABIEncodedPacked(values);
        }
    }
}
