using CryptoWallet.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoWallet.Database
{
    public class CryptoWalletDbContext : DbContext
    {
        public CryptoWalletDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}