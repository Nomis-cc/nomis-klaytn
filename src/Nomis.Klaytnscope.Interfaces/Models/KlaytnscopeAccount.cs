using System.Text.Json.Serialization;

namespace Nomis.Klaytnscope.Interfaces.Models
{
    /// <summary>
    /// Klaytnscope account.
    /// </summary>
    public class KlaytnscopeAccount
    {
        /// <summary>
        /// Success.
        /// </summary>
        [JsonPropertyName("success")]
        public bool? Success { get; set; }

        /// <summary>
        /// Code.
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        /// Account data.
        /// </summary>
        [JsonPropertyName("result")]
        public KlaytnscopeAccountData? Result { get; set; }
    }
}