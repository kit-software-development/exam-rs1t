using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoWallet.Database;
using CryptoWallet.Database.Entities;
using CryptoWallet.Helpers;
using Microsoft.EntityFrameworkCore;
using NBitcoin;
using QBitNinja.Client;

namespace CryptoWallet.Services
{
    public interface IWalletsService
    {
        Task<string> CreateWallet(string userEmail);
        Task<Dictionary<string, decimal>> GetWalletBalances(string userEmail);
        Task<List<string>> GetWalletAddresses(string userEmail);
    }

    public class WalletsService : IWalletsService
    {
        private readonly CryptoWalletDbContext _db;

        public WalletsService(CryptoWalletDbContext db)
        {
            _db = db;
        }

        public async Task<string> CreateWallet(string email)
        {
            var user = await _db.Users
                                .FirstOrDefaultAsync(d => d.Email == email);

            var privateKey = new Key();
            var wif = privateKey.GetWif(Network.TestNet).ToString(); // todo add network to config

            _db.Wallets.Add(new Wallet
            {
                User = user,
                Wif = wif,
            });
            await _db.SaveChangesAsync();

            return privateKey.PubKey.GetAddress(ScriptPubKeyType.Legacy, Network.TestNet).ToString();
        }

        public async Task<List<string>> GetWalletAddresses(string email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(d => d.Email == email);

            var addresses = await _db.Wallets
                                     .Where(w => w.User.Id == user.Id)
                                     .Select(w => w.Wif.GetAddress())
                                     .ToListAsync();

            return addresses;
        }

        public async Task<Dictionary<string, decimal>> GetWalletBalances(string email)
        {
            var client = new QBitNinjaClient(Network.TestNet);

            var user = await _db.Users.FirstOrDefaultAsync(d => d.Email == email);
            var wifs = await _db.Wallets
                                .Where(w => w.User.Id == user.Id)
                                .Select(w => w.Wif)
                                .ToListAsync();

            var balances = new Dictionary<string, decimal>();

            foreach (var wif in wifs)
            {
                var address = new BitcoinPubKeyAddress(wif.GetAddress());
                var balanceSummary = await client.GetBalanceSummary(address);
                balances.AddOrReplace(address.ToString(), balanceSummary.Spendable.Amount.ToUnit(MoneyUnit.BTC));
            }

            return balances;
        }
    }
}