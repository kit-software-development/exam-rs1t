using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoWallet.Database.Entities
{
    public class Wallet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Represents wallet Private Key in form of Wallet Input Format (Base58Check).
        /// All other data about wallet (such as address) can be derived from it,
        /// therefore it is sufficient to store only WIF in the database.
        /// </summary>
        [Required]
        public string Wif { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
    }
}