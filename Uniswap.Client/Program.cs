using System.Configuration;
using System.Threading.Tasks;
using Nethereum.Signer;
using Uniswap.Client.Blockchains;

namespace Uniswap.Client
{
    class Program
    {
        static readonly string InfuraProjectId = ConfigurationManager.AppSettings["InfuraProjectId"];

        static readonly EthereumClient Client = new EthereumClient(Chain.MainNet, InfuraProjectId);

        static async Task Main(string[] args)
        {
            var liquidity = await Client.GetPoolLiquidity();
        }
    }
}
