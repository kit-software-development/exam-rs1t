using System.Threading.Tasks;
using CryptoWallet.Model.Requests;
using CryptoWallet.Model.Responses;
using CryptoWallet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoWallet.Controllers
{
    [Authorize]
    public class TransactionsController : MyControllerBase
    {
        private readonly ITransactionsService _transactionsService;

        public TransactionsController(ITransactionsService service)
        {
            _transactionsService = service;
        }

        [HttpGet]
        public async Task<TransactionHistoryResponse> GetTransactions()
        {
            var transactionHistory = await _transactionsService.GetTransactions(User.Identity.Name);
            return new TransactionHistoryResponse
            {
                TransactionHistory = transactionHistory
            };
        }

        [HttpPost]
        public async Task<string> SendTransaction(SendTransactionRequest request)
        {
            return await _transactionsService.Send(User.Identity.Name, request.FromAddress, request.ToAddress, request.Amount, request.Fee);
        }
    }
}