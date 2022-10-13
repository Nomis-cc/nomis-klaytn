using Nomis.Utils.Contracts.Common;

namespace Nomis.Klaytnscope.Interfaces.Settings
{
    /// <summary>
    /// Klaytnscope settings.
    /// </summary>
    public class KlaytnscopeSettings :
        ISettings
    {
        /// <summary>
        /// API base URL.
        /// </summary>
        /// <remarks>
        /// <see href="https://docs.klaytn.foundation"/>
        /// </remarks>
        public string? ApiBaseUrl { get; set; }
    }
}