using System.ComponentModel.DataAnnotations;

namespace CryptoWallet.Model.Requests
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(maximumLength: 256, MinimumLength = 8)]
        public string Password { get; set; }
    }
}