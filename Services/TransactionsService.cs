using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using CryptoWallet.Database;
using CryptoWallet.Helpers;
using CryptoWallet.Model.Responses;
using Microsoft.EntityFrameworkCore;
using NBitcoin;
using QBitNinja.Client;
using Transaction = NBitcoin.Transaction;

namespace CryptoWallet.Services
{
    public interface ITransactionsService
    {
        // Creates, signs and broadcasts transaction to blockchain
        Task<string> Send(string email, string fromAddress, string toAddress, decimal amount, decimal fee);

        // Returns list of transactions for each of user addresses
        Task<Dictionary<string, List<TransactionInfo>>> GetTransactions(string email);
    }

    public class TransactionsService : ITransactionsService
    {
        private readonly CryptoWalletDbContext _db;

        public TransactionsService(CryptoWalletDbContext db)
        {
            _db = db;
        }

        public async Task<string> Send(string email, string fromAddress, string toAddress, decimal amount, decimal fee)
        {
            var user = await _db.Users.FirstOrDefaultAsync(d => d.Email == email);
            var wifs = await _db.Wallets
                                .Where(w => w.User.Id == user.Id)
                                .OrderBy(w => w.CreatedAt)
                                .Select(w => w.Wif)
                                .ToListAsync();

            BitcoinSecret privateKey = null;
            foreach (var wif in wifs)
            {
                var addressToCheck = new BitcoinPubKeyAddress(wif.GetAddress());
                if (addressToCheck.ToString().Equals(fromAddress))
                    privateKey = new BitcoinSecret(wif);
            }

            if (privateKey == null)
                throw new TransactionException("From address is invalid");

            var client = new QBitNinjaClient(Network.TestNet);
            var address = privateKey.GetAddress(ScriptPubKeyType.Legacy);

            var outPointsToSpend = new List<OutPoint>();
            var coinsToSpend = new List<ICoin>();
            var txInAmount = new Money(0, MoneyUnit.BTC);
            var balanceModel = await client.GetBalance(address, true);
            foreach (var operation in balanceModel.Operations)
            {
                var receivedCoins = operation.ReceivedCoins;
                foreach (var coin in receivedCoins)
                {
                    if (coin.TxOut.ScriptPubKey == address.ScriptPubKey)
                    {
                        outPointsToSpend.Add(coin.Outpoint);
                        coinsToSpend.Add(coin);
                        txInAmount += (Money) coin.Amount;
                    }
                }
            }

            var toBitcoinAddress = BitcoinAddress.Create(toAddress, Network.TestNet);
            var toAmount = new Money(amount, MoneyUnit.BTC);

            var minerFee = new Money(fee, MoneyUnit.BTC);

            var changeAmount = txInAmount - toAmount - minerFee;

            var txOut = new TxOut
            {
                Value = toAmount,
                ScriptPubKey = toBitcoinAddress.ScriptPubKey
            };

            var changeTxOut = new TxOut
            {
                Value = changeAmount,
                ScriptPubKey = address.ScriptPubKey
            };
            var transaction = Transaction.Create(Network.TestNet);
            transaction.Outputs.Add(txOut);
            transaction.Outputs.Add(changeTxOut);

            transaction.Inputs.AddRange(outPointsToSpend.Select(
                outPoint => new TxIn
                {
                    PrevOut = outPoint
                }));

            transaction.Inputs[0].ScriptSig = privateKey.GetAddress(ScriptPubKeyType.Legacy).ScriptPubKey;
            transaction.Sign(privateKey, coinsToSpend.ToArray());

            var broadcastResponse = await client.Broadcast(transaction);

            return broadcastResponse.Success
                ? $"Success! Go to https://insight.bitcore.io/#/BTC/testnet/tx/{transaction.GetHash()} to view transaction"
                : broadcastResponse.Error.Reason;
        }

        public async Task<Dictionary<string, List<TransactionInfo>>> GetTransactions(string email)
        {
            var client = new QBitNinjaClient(Network.TestNet);

            var user = await _db.Users.FirstOrDefaultAsync(d => d.Email == email);
            var wifs = await _db.Wallets
                                .Where(w => w.User.Id == user.Id)
                                .OrderBy(w => w.CreatedAt)
                                .Select(w => w.Wif)
                                .ToListAsync();

            var history = new Dictionary<string, List<TransactionInfo>>();

            foreach (var wif in wifs)
            {
                var address = new BitcoinPubKeyAddress(wif.GetAddress());
                var balanceModel = await client.GetBalance(address);
                var transactions = new List<TransactionInfo>();
                foreach (var operation in balanceModel.Operations)
                {
                    var transactionInfo = new TransactionInfo
                    {
                        IsSent = operation.Amount < 0,
                        Amount = operation.Amount.ToUnit(MoneyUnit.BTC),
                        Confirmations = operation.Confirmations,
                        TransactionId = operation.TransactionId.ToString()
                    };
                    if (transactionInfo.IsSent)
                        transactionInfo.Amount *= -1;
                    transactions.Add(transactionInfo);
                }

                history.AddOrReplace(address.ToString(), transactions);
            }

            return history;
        }
    }
}