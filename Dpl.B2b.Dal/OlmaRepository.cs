using System;
using System.Linq;
using Dpl.B2b.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Dpl.B2b.Dal
{
    public class OlmaRepository<TEntity> : GenericRepository<TEntity, OlmaDbContext>
        where TEntity : class
    {
        public OlmaRepository(OlmaDbContext repositoryContext) : base(repositoryContext)
        {
            this.Context.Database.SetCommandTimeout(300);
        }
    }
}
