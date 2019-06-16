using System.Collections.Generic;

namespace CryptoWallet.Model.Responses
{
    public class TransactionHistoryResponse
    {
        /// <summary>
        /// Transaction history for each of user addresses
        /// </summary>
        public Dictionary<string, List<TransactionInfo>> TransactionHistory { get; set; }
    }

    public class TransactionInfo
    {
        public bool IsSent { get; set; }

        public decimal Amount { get; set; }

        public string TransactionId { get; set; }

        public int Confirmations { get; set; }
    }
}