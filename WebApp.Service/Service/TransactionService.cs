using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Data.Entities;
using WebApp.Data.ViewModel;
using WebApp.Repository.Interface;
using WebApp.Service.IService;

namespace WebApp.Service.Service
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<TransactionViewModel> GetAll()
        {
            var viewModel = new List<TransactionViewModel>();
            try
            {
                var model = _unitOfWork.Repository<Transaction>().Query()
                    .Filter(x => x.IsActive)
                    .Get().ToList();
                foreach (var transaction in model)
                {
                    //manual mapping
                    var trans = new TransactionViewModel
                    {
                        TransactionId = transaction.TransactionId,
                        Payment = Convert.ToString(transaction.Amount) + transaction.CurrencyCode,
                        Status = transaction.Status
                    };
                    viewModel.Add(trans);
                }

                return viewModel;
            }
            catch (Exception err)
            {
                return viewModel;
            }
        }
    }
}
