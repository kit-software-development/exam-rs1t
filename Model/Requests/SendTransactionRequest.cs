using System.ComponentModel.DataAnnotations;

namespace CryptoWallet.Model.Requests
{
    public class SendTransactionRequest
    {
        [Required]
        public string FromAddress { get; set; }

        [Required]
        public string ToAddress { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public decimal Fee { get; set; }
    }
}