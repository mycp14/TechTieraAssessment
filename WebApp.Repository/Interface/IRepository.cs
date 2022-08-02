using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Data.Entities;
using WebApp.Repository.Impl;

namespace WebApp.Repository.Interface
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        /// get the entire row in the table
        /// </summary>
        /// <param name="id"></param>
        TEntity GetById(object id);

        /// <summary>
        /// add the data to specified table
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);

        /// <summary>
        /// update the data to specified table
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        /// set isactive 0 to the specified id in the table
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);

        void Restore(TEntity entity);

        /// <summary>
        /// hard delete of the row
        /// </summary>
        /// <param name="entity"></param>
        void Remove(TEntity entity);
        IEnumerable<TEntity> GetAllInactive();

        /// <summary>
        /// returns collection all active data
        /// </summary>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// initialize the repository query
        /// </summary>
        RepositoryQuery<TEntity> Query();

        //void TrackedEntities(DbChangeTracker changeTracker);
    }
}
