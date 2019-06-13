using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoWallet.Database.Entities
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}