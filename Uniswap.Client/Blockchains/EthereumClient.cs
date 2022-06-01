using System.Threading.Tasks;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Uniswap.Contracts.UniswapV3Pool;
using System.Numerics;

namespace Uniswap.Client.Blockchains
{
    /**
     * Service to write message in Ethereum blocks
     */
    public class EthereumClient
    {
        private readonly UniswapV3PoolService _poolService;

        /// <summary>
        /// Readonly constructor
        /// </summary>
        public EthereumClient(Chain network, string infuraProjectId)
        {
            var web3 = new Web3(
                GetInfuraUrl(network, infuraProjectId));
            _poolService = new UniswapV3PoolService(web3, "0x4e68ccd3e89f51c3074ca5072bbac773960dfa36");
        }

        /// <summary>
        /// Full constructor
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="network"></param>
        /// <param name="infuraProjectId"></param>
        public EthereumClient(
            string privateKey,
            Chain network,
            string infuraProjectId)
        {
            var account = new Account(privateKey, network);
            var web3 = new Web3(account, GetInfuraUrl(network, infuraProjectId));

            _poolService = new UniswapV3PoolService(web3, "0x4e68ccd3e89f51c3074ca5072bbac773960dfa36");
        }

        internal static string GetInfuraUrl(Chain network, string infuraId)
        {
            return $"https://{network}.infura.io/v3/{infuraId}";
        }

        public async Task<BigInteger> GetPoolLiquidity()
        {
            return await _poolService.LiquidityQueryAsync();
        }
    }
}