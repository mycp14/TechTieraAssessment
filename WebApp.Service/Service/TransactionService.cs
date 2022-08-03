using ISO._4217;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                        Payment = $"{Convert.ToString(transaction.Amount)} {transaction.CurrencyCode}",
                        Status = StatusId(transaction.Status)
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
        private string StatusId(string status)
        {
            string temp = "D";

            switch (status.ToLowerInvariant())
            {
                case "approved":
                    temp = "A";
                    break;
                case "rejected":
                    temp = "R";
                    break;
                default:
                    temp = "D";
                    break;
            }

            return temp;
        }
        public bool IsValidStatus(string status)
        {
            bool temp = false;

            switch (status.ToLowerInvariant())
            {
                case "approved":
                    temp = true;
                    break;
                case "rejected":
                    temp = true;
                    break;
                case "done":
                    temp = true;
                    break;
                default:
                    temp = false;
                    break;
            }

            return temp;
        }
        public bool Save(UploadTransactionViewModel viewModel)
        {
            try
            {
                //manual mapping
                var transaction = new Transaction
                {
                    TransactionId = viewModel.TransactionId,
                    Amount = viewModel.Amount,
                    TransactionDate = viewModel.TransactionDate,
                    CurrencyCode = viewModel.CurrencyCode,
                    Status = viewModel.Status,
                    CreatedDate = DateTime.Now,
                };

                _unitOfWork.Repository<Transaction>().Add(transaction);
                _unitOfWork.Save();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool isEmptyField(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return true;
            }
            return false;
        }
        public bool IsValidCurrency(string code)
        {
            //RegionInfo region = CultureInfo
            //    .GetCultures(CultureTypes.SpecificCultures)
            //    .Select(ct => new RegionInfo(ct.LCID))
            //    .Where(ri => ri.ISOCurrencySymbol == currency).FirstOrDefault();
            var currencies = CurrencyCodesResolver.GetCurrenciesByCode(code);
            if (currencies.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
