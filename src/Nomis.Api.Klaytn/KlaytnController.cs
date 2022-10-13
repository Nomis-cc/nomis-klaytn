using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nomis.Api.Klaytn.Abstractions;
using Nomis.Klaytnscope.Interfaces;
using Nomis.Klaytnscope.Interfaces.Models;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Klaytn
{
    /// <summary>
    /// A controller to aggregate all Klaytn-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Klaytn.")]
    internal sealed partial class KlaytnController :
        KlaytnBaseController
    {
        private readonly ILogger<KlaytnController> _logger;
        private readonly IKlaytnscopeService _klaytnscopeService;

        /// <summary>
        /// Initialize <see cref="KlaytnController"/>.
        /// </summary>
        /// <param name="klaytnscopeService"><see cref="IKlaytnscopeService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public KlaytnController(
            IKlaytnscopeService klaytnscopeService,
            ILogger<KlaytnController> logger)
        {
            _klaytnscopeService = klaytnscopeService ?? throw new ArgumentNullException(nameof(klaytnscopeService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get Nomis Score for given wallet address.
        /// </summary>
        /// <param name="address" example="0x0b5a570f2e9e05896c8991593fe15e0b4b89e647">Klaytn wallet address to get Nomis Score.</param>
        /// <returns>An NomisScore value and corresponding statistical data.</returns>
        /// <remarks>
        /// Sample request:
        ///     GET /api/v1/klaytn/wallet/0x0b5a570f2e9e05896c8991593fe15e0b4b89e647/score
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetKlaytnWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetKlaytnWalletScore",
            Tags = new[] { KlaytnTag })]
        [ProducesResponseType(typeof(Result<KlaytnWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetKlaytnWalletScoreAsync(
            [Required(ErrorMessage = "Wallet address should be set")] string address)
        {
            var result = await _klaytnscopeService.GetWalletStatsAsync(address);
            return Ok(result);
        }
    }
}