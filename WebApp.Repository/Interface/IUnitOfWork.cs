using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Data.Entities;

namespace WebApp.Repository.Interface
{
    public interface IUnitOfWork
    {
        void Save(); // save/commit the changes to database
        IRepository<T> Repository<T>() where T : BaseEntity; // call the specified method to repository
        void Dispose(bool disposing); // dispose the connection
    }
}
