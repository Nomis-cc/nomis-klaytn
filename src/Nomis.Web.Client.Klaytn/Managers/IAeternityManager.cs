using Nomis.Klaytnscope.Interfaces.Models;
using Nomis.Utils.Wrapper;
using Nomis.Web.Client.Common.Managers;

namespace Nomis.Web.Client.Klaytn.Managers
{
    /// <summary>
    /// Klaytn manager.
    /// </summary>
    public interface IKlaytnManager :
        IManager
    {
        /// <summary>
        /// Get klaytn wallet score.
        /// </summary>
        /// <param name="address">Wallet address.</param>
        /// <returns>Returns result of <see cref="KlaytnWalletScore"/>.</returns>
        Task<IResult<KlaytnWalletScore>> GetWalletScoreAsync(string address);
    }
}