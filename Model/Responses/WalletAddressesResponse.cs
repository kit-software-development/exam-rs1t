using System.Collections.Generic;

namespace CryptoWallet.Model.Responses
{
    public class WalletAddressesResponse
    {
        /// <summary>
        /// Addresses assigned to user
        /// </summary>
        public List<string> Addresses { get; set; }
    }
}