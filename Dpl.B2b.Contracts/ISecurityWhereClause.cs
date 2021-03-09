using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Dpl.B2b.Contracts
{
    public interface ISecurityWhereClause<TEntity>
    {
        Expression<Func<TEntity, bool>> GetSecurityWhereClause();
    }
}
