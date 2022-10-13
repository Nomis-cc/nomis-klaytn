using Nomis.Klaytnscope.Interfaces.Models;
using Nomis.Utils.Contracts.Services;
using Nomis.Utils.Wrapper;

namespace Nomis.Klaytnscope.Interfaces
{
    /// <summary>
    /// Klaytnscope service.
    /// </summary>
    public interface IKlaytnscopeService :
        IInfrastructureService
    {
        /// <summary>
        /// Client for interacting with Klaytnscope API.
        /// </summary>
        public IKlaytnscopeClient Client { get; }

        /// <summary>
        /// Get klaytn wallet stats by address.
        /// </summary>
        /// <param name="address">Klaytn wallet address.</param>
        /// <returns>Returns <see cref="KlaytnWalletScore"/> result.</returns>
        public Task<Result<KlaytnWalletScore>> GetWalletStatsAsync(string address);
    }
}