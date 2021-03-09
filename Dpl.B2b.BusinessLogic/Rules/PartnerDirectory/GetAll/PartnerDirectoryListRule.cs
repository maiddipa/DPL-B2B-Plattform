using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.PartnerDirectory.GetAll
{
    public class PartnerDirectorListRule : BaseValidationWithServiceProviderRule<PartnerDirectorListRule, PartnerDirectorListRule.ContextModel>
    {
        public PartnerDirectorListRule(GetAll.MainRule.ContextModel context, IRule parentRule)
        {
            // Create Context
            Context = new ContextModel(context, this);
            ParentRule = parentRule;
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
            var repository = ServiceProvider.GetService<IRepository<Olma.PartnerDirectory>>();
            
            var query = repository.FindAll()
                .Include(pd => pd.CustomerPartnerDirectoryAccesses)
                .ThenInclude(a => a.Partner)
                .ThenInclude(p => p.Address)
                .AsNoTracking();

            #region ordering

            // handle ordering, which could also be based on request
            query = query.OrderBy(i => i.Name);

            #endregion
            
            var partnerDirectories = query.ToList();
            var mapper = ServiceProvider.GetService<IMapper>();

            Context.MappedResult = mapper.Map<List<Contracts.Models.PartnerDirectory>>(partnerDirectories);
            // Context.MappedResult = new PartnerDirectoryResult
            // {
            //     PartnerDirectories = mapper.Map<List<Contracts.Models.PartnerDirectory>>(partnerDirectories),
            //     Partners = mapper.Map<List<Contracts.Models.CustomerPartner>>(
            //             partnerDirectories
            //                 .SelectMany(partnerDirectory => partnerDirectory.CustomerPartnerDirectoryAccesses)
            //                 .Select(p => p.Partner))
            //         .ToList()
            // };
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<object>
        {
            public ContextModel(GetAll.MainRule.ContextModel context, IRule rule) : base(context, rule)
            {

            }
            //public PartnerDirectoryResult MappedResult { get; protected internal set; }

            public IList<Contracts.Models.PartnerDirectory> MappedResult { get; protected internal set; }
        }

        #endregion
    }
}