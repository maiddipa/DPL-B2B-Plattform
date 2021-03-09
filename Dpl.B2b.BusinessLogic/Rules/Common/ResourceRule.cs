using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using Z.EntityFramework.Plus;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.Common
{
    public class ResourceRule<T> : BaseValidationWithServiceProviderRule<ResourceRule<T>, ResourceRule<T>.ContextModel> where T:class
    {
        public ResourceRule(int resourceId, IRule parentRule, ILocalizableMessage message=null)
        {
            // Create Context
            Context = new ContextModel((resourceId, null), this);
            ParentRule = parentRule;

            _buildMessage = message == null 
                ? (Func<ILocalizableMessage>) MessageMap 
                : () => message;
        }
        
        /// <summary>
        /// If there is no resource id and resource is searched by predicate 
        /// </summary>
        /// <param name="predicate">the where clause</param>
        /// <param name="parentRule">parent rule</param>
        /// <param name="message">custom message</param>
        public ResourceRule(Expression<Func<T,bool>> predicate, IRule parentRule, ILocalizableMessage message=null)
        {
            // Create Context
            Context = new ContextModel((null, predicate), this);
            ParentRule = parentRule;

            _buildMessage = message == null 
                ? (Func<ILocalizableMessage>) MessageMap 
                : () => message;
        }

        private bool _asNoTracking;
        
        /// <summary>
        /// Include EagleLoad Expressions
        /// </summary>
        /// <param name="include"></param>
        /// <returns></returns>
        public ResourceRule<T> Include(Expression<Func<T,object>> include)
        {
            _include.Add(include);
            return this;
        }

        /// <summary>
        /// Include multiple EagleLoad Expressions
        /// </summary>
        /// <param name="include"></param>
        /// <returns></returns>
        public ResourceRule<T> IncludeAll(IEnumerable<Expression<Func<T,object>>> include)
        {
            _include.AddRange(include);
            return this;
        }

        private Func<IQueryable<T>, IIncludableQueryable<T, object>> _includeFunc;

        /// <summary>
        /// Assigning a specific EagleLoading Function to be used when loading the data
        /// Use Include first on the input in the function./
        /// </summary>
        /// <example>
        /// .UseEagleLoad(t => t.Include(p => p.Bids).ThenInclude(p => p.Division));
        /// </example>
        /// <param name="func"></param>
        /// <returns></returns>
        public ResourceRule<T> UseEagleLoad(Func<IQueryable<T>, IIncludableQueryable<T, object>> func)
        {
            _includeFunc = func;
            return this;
        }

        public ResourceRule<T> AsNoTracking()
        {
            _asNoTracking = true;
            return this;
        }

        

        public ResourceRule<T> IgnoreQueryFilter()
        {
            _ignoreQueryFilter = true;
            return this;
        }
        
        private readonly List<Expression<Func<T, object>>> _include = new List<Expression<Func<T, object>>>();
       
        /// <summary>
        /// Message for RuleState if Rule is invalid 
        /// </summary>
        protected override ILocalizableMessage Message => _buildMessage();

        private readonly Func<ILocalizableMessage> _buildMessage;
        private bool _ignoreQueryFilter;

        private ILocalizableMessage MessageMap()
        {
            return typeof(T) switch
            {
                { } partnerDirectory when partnerDirectory == typeof(Olma.PartnerDirectory) => new PartnerDirectoryNotFound(),
                _ => new ResourceNotFound()
            };
        }

        /// <summary>
        /// Internal Method for Evaluate
        /// </summary>
        protected override void EvaluateInternal()
        {
            // Initialized all rules. Instancing only takes place during execution, since otherwise the service scope has not yet been assigned
            Context.Rules = new RulesBundle(Context, this);
            
            var repository = ServiceProviderServiceExtensions.GetService<IRepository<T>>(ServiceProvider);
            
            var queryable = repository.FindAll();
            
            // IgnoreQueryFilters
            if (_ignoreQueryFilter)
                queryable = queryable.IgnoreQueryFilters();
            
            // Eagle Load
            if (_includeFunc != null)
            {
                queryable = _includeFunc(queryable);    
            }
            
            queryable = _include.Aggregate(queryable, (current, include) => current.Include(include));
            
            var query = queryable.Where(Context.Predicate ?? (i => EF.Property<int>(i, "Id") == Context.Id));

            // Alternativ per DynamicQueryable
            //var query = queryable.Where("ID == @0", Context.Id);


            if (_asNoTracking) 
                query = query.AsNoTracking();
            
            Context.Resource =  query.FirstOrDefault();
            
            AddMessage(Context.Resource==null, ResourceName, Message);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<(int? id, Expression<Func<T,bool>> predicate)>
        {
            public ContextModel((int? id, Expression<Func<T,bool>> predicate) parentContext, IRule rule) : base(parentContext, rule)
            {
                Predicate = parentContext.predicate;
            }

            public int? Id => Parent.id;
            
            public RulesBundle Rules { get; protected internal set; }
            
            public T Resource { get; protected internal set; }
            public Expression<Func<T,bool>> Predicate { get; set; }
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, IRule rule)
            {

            }
        }

        #endregion
    }
}