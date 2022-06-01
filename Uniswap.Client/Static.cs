using System.Collections.Generic;

namespace Uniswap.Client
{
    public static class Static
    {
        public static Dictionary<string, Coin> SupportedCoins { get; set; }
            = new Dictionary<string, Coin>();
    }

    public class Coin
    {
        public decimal Usd { get; set; }
        public string Ticker { get; set; }
        public int DecimalPow { get; set; }
    }
}
