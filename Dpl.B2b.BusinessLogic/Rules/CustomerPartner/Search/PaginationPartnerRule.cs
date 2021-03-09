using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.CustomerPartner.Search
{
    public class PaginationPartnerRule : BaseValidationWithServiceProviderRule<PaginationPartnerRule, PaginationPartnerRule.ContextModel>
    {
        public PaginationPartnerRule(MainRule.ContextModel request, IRule parentRule)
        {
            ParentRule = parentRule;
            
            Context = new ContextModel(request, this)
            {
                Mapper = ServiceProvider.GetService<IMapper>()
            };
        }

        /// <summary>
        /// Message for RuleState if Rule is invalid 
        /// </summary>
        protected override ILocalizableMessage Message => new NotAllowedByRule();

        /// <summary>
        /// Internal Method for Evaluate
        /// </summary>
        protected override void EvaluateInternal()
        {
            Context.PaginationResult = BuildPaginationResult();

            AddMessage(Context.PaginationResult==null, ResourceName, Message);
        }

        private IPaginationResult<Contracts.Models.CustomerPartner> BuildPaginationResult()
        {
            var repository = ServiceProvider.GetService<IRepository<Olma.CustomerPartner>>();

            var query = repository.FindAll().AsNoTracking();

            #region where clauses

            if (Context.CustomerPartnerDirectoryId != null)
            {
                query = query.Where(i => i.DirectoryAccesses.Any(i => i.DirectoryId == Context.CustomerPartnerDirectoryId));
            }

            if (!string.IsNullOrEmpty(Context.CompanyName))
            {
                query = query.Where(p => p.CompanyName.Contains(Context.CompanyName));
            }

            if (!string.IsNullOrEmpty(Context.City))
            {
                query = query.Where(p => p.Address != null && p.Address.City.ToLower() == Context.City.ToLower());
            }

            if (!string.IsNullOrEmpty(Context.Country))
            {
                query = query.Where(p =>
                    p.Address != null && p.Address.Country != null && p.Address.Country.Name == Context.Country);
            }

            #endregion

            #region ordering

            query = query.OrderBy(p => p.CompanyName);

            #endregion

            var projectedQuery = query.ProjectTo<Contracts.Models.CustomerPartner>(Context.Mapper.ConfigurationProvider);
            return projectedQuery.ToPaginationResult(Context.Pagination);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<MainRule.ContextModel>
        {
            public ContextModel(MainRule.ContextModel parent, IRule rule) : base(parent, rule)
            {
                
            }

            public IMapper Mapper { get; protected internal set; }

            public int? CustomerPartnerDirectoryId => Parent.Parent.CustomerPartnerDirectoryId;
            public string CompanyName => Parent.Parent.CompanyName;
            public string Country => Parent.Parent.Country;
            public string City => Parent.Parent.City;
            public IPagination Pagination => Parent.Parent;

            public IPaginationResult<Contracts.Models.CustomerPartner> PaginationResult { get; set; }
        }
        
        #endregion
    }
}