using System.Collections.Generic;

namespace CryptoWallet.Model.Responses
{
    public class WalletBalancesResponse
    {
        /// <summary>
        /// Balance for each address
        /// </summary>
        public Dictionary<string, decimal> Balances { get; set; }
    }
}