using NBitcoin;

namespace CryptoWallet.Helpers
{
    public static class WalletExtensions
    {
        public static string GetAddress(this string wif)
        {
            return new BitcoinSecret(wif, Network.TestNet)
                   .PubKey
                   .GetAddress(ScriptPubKeyType.Legacy, Network.TestNet)
                   .ToString();
        }
    }
}