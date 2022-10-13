namespace Nomis.Klaytnscope.Interfaces.Models
{
    /// <summary>
    /// Klaytn wallet score.
    /// </summary>
    public class KlaytnWalletScore
    {
        /// <summary>
        /// Nomis Score in range of [0; 1].
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// Additional stat data used in score calculations.
        /// </summary>
        public KlaytnWalletStats? Stats { get; set; }
    }
}