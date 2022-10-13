using System.Text.Json.Serialization;

namespace Nomis.Klaytnscope.Interfaces.Models
{
    /// <summary>
    /// Klaytnscope account KIP-17 token data.
    /// </summary>
    public class KlaytnscopeAccountKIP17TokenData
    {
        /// <summary>
        /// Image.
        /// </summary>
        [JsonPropertyName("image")]
        public string? Image { get; set; }

        /// <summary>
        /// Token name.
        /// </summary>
        [JsonPropertyName("tokenName")]
        public string? TokenName { get; set; }

        /// <summary>
        /// Token symbol.
        /// </summary>
        [JsonPropertyName("tokenSymbol")]
        public string? TokenSymbol { get; set; }

        /// <summary>
        /// Whitelist flag.
        /// </summary>
        [JsonPropertyName("whitelistFlag")]
        public bool WhitelistFlag { get; set; }

        /// <summary>
        /// Verified.
        /// </summary>
        [JsonPropertyName("verified")]
        public bool Verified { get; set; }
    }
}