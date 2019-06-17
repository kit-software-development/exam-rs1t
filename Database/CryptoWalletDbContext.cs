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
        public DbSet<Wallet> Wallets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                        .HasIndex(u => u.Email)
                        .IsUnique();

            modelBuilder.Entity<Wallet>()
                        .HasIndex(u => u.Wif)
                        .IsUnique();

            modelBuilder.Entity<Wallet>()
                        .HasOne(w => w.User)
                        .WithMany(u => u.Wallets);
        }
    }
}