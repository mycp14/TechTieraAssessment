using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using WebApp.Data.Context;
using WebApp.Data.Entities;
using WebApp.Repository.Interface;

namespace WebApp.Repository.Impl
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WebAppContext _context;
        private bool _disposed;
        private Hashtable _repositories;

        public UnitOfWork(WebAppContext context)
        {
            _context = context;
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (_disposed)
                    _context.Dispose();

            _disposed = true;
        }

        public IRepository<T> Repository<T>() where T : BaseEntity
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);

                var repositoryInstance =
                    Activator.CreateInstance(repositoryType
                        .MakeGenericType(typeof(T)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<T>)_repositories[type];
        }

        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                throw;
            }
        }
    }
}
