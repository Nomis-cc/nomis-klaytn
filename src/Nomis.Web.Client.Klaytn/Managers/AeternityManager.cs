using Microsoft.Extensions.Options;
using Nomis.Klaytnscope.Interfaces.Models;
using Nomis.Utils.Wrapper;
using Nomis.Web.Client.Common.Extensions;
using Nomis.Web.Client.Common.Settings;
using Nomis.Web.Client.Klaytn.Routes;

namespace Nomis.Web.Client.Klaytn.Managers
{
    /// <inheritdoc cref="IKlaytnManager" />
    public class KlaytnManager :
        IKlaytnManager
    {
        private readonly HttpClient _httpClient;
        private readonly KlaytnEndpoints _endpoints;

        /// <summary>
        /// Initialize <see cref="KlaytnManager"/>.
        /// </summary>
        /// <param name="webApiSettings"><see cref="WebApiSettings"/>.</param>
        public KlaytnManager(
            IOptions<WebApiSettings> webApiSettings)
        {
            _httpClient = new()
            {
                BaseAddress = new(webApiSettings.Value?.ApiBaseUrl ?? throw new ArgumentNullException(nameof(webApiSettings.Value.ApiBaseUrl)))
            };
            _endpoints = new(webApiSettings.Value?.ApiBaseUrl ?? throw new ArgumentNullException(nameof(webApiSettings.Value.ApiBaseUrl)));
        }

        /// <inheritdoc />
        public async Task<IResult<KlaytnWalletScore>> GetWalletScoreAsync(string address)
        {
            var response = await _httpClient.GetAsync(_endpoints.GetWalletScore(address));
            return await response.ToResultAsync<KlaytnWalletScore>();
        }
    }
}