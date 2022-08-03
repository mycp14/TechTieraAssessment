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
        private bool IsValidStatus(string status)
        {
            bool temp = false;
            if (!isEmptyField(status))
            {
                switch (status.ToLowerInvariant())
                {
                    case "approved":
                        temp = true;
                        break;
                    case "failed":
                        temp = true;
                        break;
                    case "finished":
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
            }

            return temp;
        }
        public bool Save(List<UploadTransactionViewModel> viewModel)
        {
            try
            {
                //manual mapping
                foreach (var vm in viewModel)
                {
                    var transaction = new Transaction
                    {
                        TransactionId = vm.TransactionId,
                        Amount = Convert.ToDecimal(vm.Amount), //viewModel.Amount,
                        TransactionDate = DateTime.Parse(vm.TransactionDate),
                        CurrencyCode = vm.CurrencyCode,
                        Status = vm.Status,
                        CreatedDate = DateTime.Now,
                    };
                    _unitOfWork.Repository<Transaction>().Add(transaction);
                    _unitOfWork.Save();
                }


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool isEmptyField(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return true;
            }
            return false;
        }
        private bool IsValidCurrency(string code)
        {
            if (!isEmptyField(code))
            {
                var currencies = CurrencyCodesResolver.GetCurrenciesByCode(code);
                if (currencies.Count() > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public bool ValidateFields(UploadTransactionViewModel vm, out List<string> errorMessage)
        {
            bool isValid = true;
            errorMessage = new List<string>();

            if (isEmptyField(vm.TransactionId))
            {
                errorMessage.Add("Transaction Id is empty");
            }
            if (isEmptyField(vm.TransactionDate))
            {
                errorMessage.Add("Transaction date is empty");
            }
            if (isEmptyField(vm.Status))
            {
                errorMessage.Add("Status is empty");
            }
            if (isEmptyField(vm.Amount))
            {
                errorMessage.Add("Amount is empty");
            }
            if (isEmptyField(vm.CurrencyCode))
            {
                errorMessage.Add("Currency code is empty");
            }
            try
            {
                decimal temp = Convert.ToDecimal(vm.Amount);
            }
            catch (FormatException)
            {
                errorMessage.Add("The amount is not formatted as a decimal.");
            }
            if (!IsValidCurrency(vm.CurrencyCode))
            {
                errorMessage.Add("Invalid currency code");
            }
            if (!IsValidStatus(vm.Status))
            {
                errorMessage.Add("Invalid status");
            }

            if (errorMessage != null)
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
