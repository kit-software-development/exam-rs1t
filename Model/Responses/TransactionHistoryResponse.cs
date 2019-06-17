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
        /// <summary>
        /// If true then transaction was sent from user address, if false then transaction was received
        /// </summary>
        public bool IsSent { get; set; }

        /// <summary>
        /// Amount in BTC
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Transaction hash, can be viewed in any Bitcoin block explorer, such as https://insight.bitcore.io/
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// Number of confirmations in blockchain
        /// </summary>
        public int Confirmations { get; set; }
    }
}