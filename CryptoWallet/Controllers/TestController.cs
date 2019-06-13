using Microsoft.AspNetCore.Mvc;
using NBitcoin;

namespace CryptoWallet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("address")]
        public ActionResult<string> Get(int id)
        {
            var privateKey = new Key();
            var bitcoinSecret = privateKey.GetWif(Network.TestNet);
            var publicKey = privateKey.PubKey;
            var bitcoinAddress = publicKey.GetAddress(ScriptPubKeyType.Legacy, Network.TestNet);
            return $"Private key (WIF): {bitcoinSecret}\nAddress: {bitcoinAddress}";
        }
    }
}