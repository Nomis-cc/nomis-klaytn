using Nomis.Utils.Contracts.Common;

namespace Nomis.Blockchain.Abstractions.Settings
{
    /// <summary>
    /// API visibility settings.
    /// </summary>
    public class ApiVisibilitySettings
        : ISettings
    {
        /// <summary>
        /// Klaytn API is enabled.
        /// </summary>
        public bool KlaytnAPIEnabled { get; set; }

    }
}