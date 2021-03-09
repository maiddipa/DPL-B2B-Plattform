using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dpl.B2b.Contracts
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> FindAll();
        IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression);

        TEntity GetByKey(params object[] keyValues);

        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        void CreateMany(IEnumerable<TEntity> entities);

        /// <summary>
        /// Check for DirtyStates
        /// </summary>
        /// <returns></returns>
        bool IsDirty();

        /// <summary>
        /// Get Entities with DirtyStates 
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> QueryDirty();

        /// <summary>
        /// This saves tracked changes of all entities made on the underlying DBContext in the current scope (usually = request)
        /// </summary>
        /// <returns></returns>
        int Save();
    }
}
