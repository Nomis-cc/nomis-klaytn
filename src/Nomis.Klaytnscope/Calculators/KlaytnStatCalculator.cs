using System.Numerics;

using Nomis.Blockchain.Abstractions.Calculators;
using Nomis.Klaytnscope.Extensions;
using Nomis.Klaytnscope.Interfaces.Models;
using Nomis.Utils.Extensions;

namespace Nomis.Klaytnscope.Calculators
{
    /// <summary>
    /// Klaytn wallet stats calculator.
    /// </summary>
    internal sealed class KlaytnStatCalculator :
        IStatCalculator<KlaytnWalletStats>
    {
        private readonly string _address;
        private readonly decimal _balance;
        private readonly IEnumerable<KlaytnscopeAccountNormalTransaction> _transactions;
        private readonly IEnumerable<KlaytnscopeAccountInternalTransaction> _internalTransactions;
        private readonly IEnumerable<KlaytnscopeAccountNftTransfer> _nftTransfers;
        private readonly IEnumerable<KlaytnscopeAccountKIP17TokenEvent> _kip17TokenTransfers;

        public KlaytnStatCalculator(
            string address,
            decimal balance,
            IEnumerable<KlaytnscopeAccountNormalTransaction> transactions,
            IEnumerable<KlaytnscopeAccountInternalTransaction> internalTransactions,
            IEnumerable<KlaytnscopeAccountNftTransfer> nftTransfers,
            IEnumerable<KlaytnscopeAccountKIP17TokenEvent> kip17TokenTransfers)
        {
            _address = address;
            _balance = balance;
            _transactions = transactions;
            _internalTransactions = internalTransactions;
            _nftTransfers = nftTransfers;
            _kip17TokenTransfers = kip17TokenTransfers;
        }

        public KlaytnWalletStats GetStats()
        {
            if (!_transactions.Any())
            {
                return new()
                {
                    NoData = true
                };
            }

            var intervals = IStatCalculator<KlaytnWalletStats>
                .GetTransactionsIntervals(_transactions.Select(x => x.CreatedAt.ToString().ToDateTime())).ToList();
            if (!intervals.Any())
            {
                return new()
                {
                    NoData = true
                };
            }

            var monthAgo = DateTime.Now.AddMonths(-1);
            var yearAgo = DateTime.Now.AddYears(-1);

            var soldTokens = _nftTransfers.Where(x => x.FromAddress?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true).ToList();
            var soldSum = IStatCalculator<KlaytnWalletStats>
                .GetTokensSum(soldTokens.Select(x => x.BlockNumber.ToString()), _internalTransactions.Select(x => (x.BlockNumber.ToString(), ulong.TryParse(x.Amount, out var amount) ? amount : new BigInteger(0))));

            var soldTokensIds = soldTokens.Select(x => x.GetTokenUid());
            var buyTokens = _nftTransfers.Where(x => x.ToAddress?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true && soldTokensIds.Contains(x.GetTokenUid()));
            var buySum = IStatCalculator<KlaytnWalletStats>
                .GetTokensSum(buyTokens.Select(x => x.BlockNumber.ToString()), _internalTransactions.Select(x => (x.BlockNumber.ToString(), ulong.TryParse(x.Amount, out var amount) ? amount : new BigInteger(0))));

            var buyNotSoldTokens = _nftTransfers.Where(x => x.ToAddress?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) != true && !soldTokensIds.Contains(x.GetTokenUid()));
            var buyNotSoldSum = IStatCalculator<KlaytnWalletStats>
                .GetTokensSum(buyNotSoldTokens.Select(x => x.BlockNumber.ToString()), _internalTransactions.Select(x => (x.BlockNumber.ToString(), ulong.TryParse(x.Amount, out var amount) ? amount : new BigInteger(0))));

            var holdingTokens = _nftTransfers.Count() - soldTokens.Count;
            var nftWorth = buySum == 0 ? 0 : (decimal)soldSum / (decimal)buySum * (decimal)buyNotSoldSum;
            var contractsCreated = _transactions.Count(x => x.TxType?.Equals("contract_create", StringComparison.CurrentCultureIgnoreCase) == true);
            var totalTokens = _kip17TokenTransfers.Select(x => x.TokenAddress).Distinct();

            return new()
            {
                Balance = _balance.ToKlay(),
                WalletAge = IStatCalculator<KlaytnWalletStats>
                    .GetWalletAge(_transactions.Select(x => x.CreatedAt.ToString())),
                TotalTransactions = _transactions.Count(),
                TotalRejectedTransactions = _transactions.Count(t => t.TxStatus == 0),
                MinTransactionTime = intervals.Min(),
                MaxTransactionTime = intervals.Max(),
                AverageTransactionTime = intervals.Average(),
                WalletTurnover = _transactions.Sum(x => decimal.TryParse(x.Amount, out var amount) ? amount : 0).ToKlay(),
                LastMonthTransactions = _transactions.Count(x => x.CreatedAt.ToString().ToDateTime() > monthAgo),
                LastYearTransactions = _transactions.Count(x => x.CreatedAt.ToString().ToDateTime() > yearAgo),
                TimeFromLastTransaction = (int)((DateTime.UtcNow - _transactions.OrderBy(x => x.CreatedAt).Last().CreatedAt.ToString().ToDateTime()).TotalDays / 30),
                NftHolding = holdingTokens,
                NftTrading = (soldSum - buySum).ToKlay(),
                NftWorth = nftWorth.ToKlay(),
                DeployedContracts = contractsCreated,
                TokensHolding = totalTokens.Count()
            };
        }
    }
}