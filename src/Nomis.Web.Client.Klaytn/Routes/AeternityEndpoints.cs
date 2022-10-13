using Nomis.Web.Client.Common.Routes;

namespace Nomis.Web.Client.Klaytn.Routes
{
    /// <summary>
    /// Klaytn endpoints.
    /// </summary>
    public class KlaytnEndpoints :
        BaseEndpoints
    {
        /// <summary>
        /// Initialize <see cref="KlaytnEndpoints"/>.
        /// </summary>
        /// <param name="baseUrl">Klaytn API base URL.</param>
        public KlaytnEndpoints(string baseUrl)
            : base(baseUrl)
        {
        }

        /// <inheritdoc/>
        public override string Blockchain => "klaytn";
    }
}