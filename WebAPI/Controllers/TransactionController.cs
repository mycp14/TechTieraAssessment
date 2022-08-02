using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Data.ViewModel;
using WebApp.Service.IService;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly ITransactionService _transactionService;

        public TransactionController(ILogger<TransactionController> logger, ITransactionService transactionService)
        {
            _logger = logger;
            _transactionService = transactionService;
        }

        /// <summary>
		/// Get All Active Transaction
		/// </summary>
		/// <returns></returns>
		[HttpGet]
        public List<TransactionViewModel> GetAll()
        {
            var list = new List<TransactionViewModel>();

            return _transactionService.GetAll().ToList();
        }
    }
}
