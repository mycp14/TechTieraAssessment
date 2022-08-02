using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Data.ViewModel;

namespace WebApp.Service.IService
{
    public interface ITransactionService
    {
        IList<TransactionViewModel> GetAll();
    }
}
