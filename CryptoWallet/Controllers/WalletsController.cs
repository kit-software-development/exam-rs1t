using System.Threading.Tasks;
using CryptoWallet.Model.Responses;
using CryptoWallet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoWallet.Controllers
{
    [Authorize]
    public class WalletsController : MyControllerBase
    {
        private readonly IWalletsService _walletsService;

        public WalletsController(IWalletsService service)
        {
            _walletsService = service;
        }

        [HttpGet]
        public async Task<WalletAddressesResponse> GetWalletAddresses()
        {
            var addresses = await _walletsService.GetWalletAddresses(User.Identity.Name);
            return new WalletAddressesResponse
            {
                Addresses = addresses
            };
        }

        [HttpPost]
        public async Task<string> CreateWallet()
        {
            return await _walletsService.CreateWallet(User.Identity.Name);
        }

        [HttpGet("balances")]
        public async Task<WalletBalancesResponse> GetWalletBalances()
        {
            var balances = await _walletsService.GetWalletBalances(User.Identity.Name);
            return new WalletBalancesResponse
            {
                Balances = balances
            };
        }
    }
}