using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dpl.B2b.Contracts;

namespace Dpl.B2b.Dal
{

    public class GenericRepository<TEntity, TDbContext> : IRepository<TEntity>
        where TEntity : class
        where TDbContext : DbContext
    {
        protected TDbContext Context { get; set; }
        public static EntityState[] DirtyStates = new[] {EntityState.Added, EntityState.Deleted, EntityState.Modified};

        public GenericRepository(TDbContext context)
        {
            this.Context = context;
        }

        public IQueryable<TEntity> FindAll()
        {
            return this.Context.Set<TEntity>();
        }

        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression)
        {
            return this.Context.Set<TEntity>()
                .Where(expression);
        }

        public TEntity GetByKey(params object[] keyValues)
        {
            return this.Context.Set<TEntity>().Find(keyValues);
        }

        public void Create(TEntity entity)
        {
            this.Context.Set<TEntity>().Add(entity);
        }

        public void Update(TEntity entity)
        {
            this.Context.Set<TEntity>().Update(entity);
        }

        public void Delete(TEntity entity)
        {
            this.Context.Set<TEntity>().Remove(entity);
        }

        public bool IsDirty()
        {
            return QueryDirty().Any();
        }
        
        public IEnumerable<TEntity> QueryDirty()
        {
            Context.ChangeTracker.DetectChanges();
            return Context.ChangeTracker
                .Entries<TEntity>()
                .Where(e=>DirtyStates.Contains(e.State))
                .Select(e=>e.Entity);
        }

        public virtual int Save()
        {
            return this.Context.SaveChanges();
        }

        public void CreateMany(IEnumerable<TEntity> entities)
        {
            this.Context.Set<TEntity>().AddRange(entities);
        }
    }
}
