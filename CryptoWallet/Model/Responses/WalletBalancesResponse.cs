using System.Collections.Generic;

namespace CryptoWallet.Model.Responses
{
    public class WalletBalancesResponse
    {
        public Dictionary<string, decimal> Balances { get; set; }
    }
}