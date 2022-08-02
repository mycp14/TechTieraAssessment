using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using WebApp.Data.Context;
using WebApp.Data.Entities;
using WebApp.Repository.Interface;

namespace WebApp.Repository.Impl
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected WebAppContext DbContext;
        internal DbSet<TEntity> DbSet;

        public Repository(WebAppContext context)
        {
            DbContext = context;
            DbSet = context.Set<TEntity>();
        }
        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public void Delete(TEntity entity)
        {
            entity.IsActive = false;
            entity.DeletedDate = DateTime.Now;
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Restore(TEntity entity)
        {
            entity.IsActive = true;
            entity.RestoredDate = DateTime.Now;
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DbSet.Where(m => m.IsActive).AsQueryable();
        }

        public IEnumerable<TEntity> GetAllInactive()
        {
            return DbSet.Where(m => !m.IsActive).AsQueryable();
        }

        public TEntity GetById(object id)
        {
            return DbSet.Find(id);
        }

        public virtual void Update(TEntity entity)
        {
            //entity.UpdatedDate = DateTime.Now;
            DbContext.Entry(entity).State = EntityState.Modified;

        }

        public virtual RepositoryQuery<TEntity> Query()
        {
            var repositoryGetFluentHelper =
                new RepositoryQuery<TEntity>(this);

            return repositoryGetFluentHelper;
        }

        internal IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
                IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>>
                includeProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (includeProperties != null)
                includeProperties.ForEach(i => { query = query.Include(i); });

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            if (page != null && pageSize != null)
                query = query
                    .Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);

            return query;
        }

    }
}
