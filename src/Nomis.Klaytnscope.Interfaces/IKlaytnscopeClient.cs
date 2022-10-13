using Nomis.Klaytnscope.Interfaces.Models;

namespace Nomis.Klaytnscope.Interfaces
{
    /// <summary>
    /// Klaytnscope client.
    /// </summary>
    public interface IKlaytnscopeClient
    {
        /// <summary>
        /// Get the account balance in Wei.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns <see cref="KlaytnscopeAccount"/>.</returns>
        Task<KlaytnscopeAccount> GetBalanceAsync(string address);

        /// <summary>
        /// Get list of specific transactions of the given account.
        /// </summary>
        /// <typeparam name="TResult">The type of returned response.</typeparam>
        /// <typeparam name="TResultItem">The type of returned response data items.</typeparam>
        /// <param name="address">Account address.</param>
        /// <returns>Returns list of specific transactions of the given account.</returns>
        Task<IEnumerable<TResultItem>> GetTransactionsAsync<TResult, TResultItem>(string address)
            where TResult : IKlaytnscopeTransferList<TResultItem>
            where TResultItem : class;
    }
}