using Nomis.Klaytnscope.Calculators;
using Nomis.Klaytnscope.Extensions;
using Nomis.Klaytnscope.Interfaces;
using Nomis.Klaytnscope.Interfaces.Models;
using Nomis.Utils.Contracts.Services;
using Nomis.Utils.Wrapper;

namespace Nomis.Klaytnscope
{
    /// <inheritdoc cref="IKlaytnscopeService"/>
    internal sealed class KlaytnscopeService :
        IKlaytnscopeService,
        ITransientService
    {
        /// <summary>
        /// Initialize <see cref="KlaytnscopeService"/>.
        /// </summary>
        /// <param name="client"><see cref="IKlaytnscopeClient"/>.</param>
        public KlaytnscopeService(
            IKlaytnscopeClient client)
        {
            Client = client;
        }

        /// <inheritdoc/>
        public IKlaytnscopeClient Client { get; }

        /// <inheritdoc/>
        public async Task<Result<KlaytnWalletScore>> GetWalletStatsAsync(string address)
        {
            var balanceWei = (await Client.GetBalanceAsync(address)).Result?.Balance;
            var kip17Tokens = (await Client.GetTransactionsAsync<KlaytnscopeAccountKIP17TokenEvents, KlaytnscopeAccountKIP17TokenEvent>(address)).ToList();
            var nftTransfers = (await Client.GetTransactionsAsync<KlaytnscopeAccountNftTransfers, KlaytnscopeAccountNftTransfer>(address)).ToList();
            var transactions = (await Client.GetTransactionsAsync<KlaytnscopeAccountNormalTransactions, KlaytnscopeAccountNormalTransaction>(address)).ToList();
            var internalTransactions = (await Client.GetTransactionsAsync<KlaytnscopeAccountInternalTransactions, KlaytnscopeAccountInternalTransaction>(address)).ToList();
            // var kip37Tokens = (await Client.GetTransactionsAsync<KlaytnscopeAccountKIP37TokenEvents, KlaytnscopeAccountKIP37TokenEvent>(address)).ToList();

            var walletStats = new KlaytnStatCalculator(
                    address,
                    decimal.TryParse(balanceWei, out var balance) ? balance : 0,
                    transactions,
                    internalTransactions,
                    nftTransfers,
                    kip17Tokens)
                .GetStats();

            return await Result<KlaytnWalletScore>.SuccessAsync(new()
            {
                Stats = walletStats,
                Score = walletStats.GetScore()
            }, "Got klaytn wallet score.");
        }
    }
}