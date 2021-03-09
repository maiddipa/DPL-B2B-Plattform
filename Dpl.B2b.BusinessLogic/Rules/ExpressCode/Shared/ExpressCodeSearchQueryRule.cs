using System;
using System.Linq;
using AutoMapper;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Warnings.Common;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.ExpressCode.Shared
{
    public class ExpressCodeSearchQueryRule : BaseValidationWithServiceProviderRule<ExpressCodeSearchQueryRule, ExpressCodeSearchQueryRule.ContextModel>
    {
        public ExpressCodeSearchQueryRule(ExpressCodesSearchRequest request, IRule parentRule)
        {
            // Create Context
            Context = new ContextModel(request, this);
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
            // Initialized all rules. Instancing only takes place during execution, since otherwise the service scope has not yet been assigned
            Context.Rules = new RulesBundle(Context, this);
            Context.AuthData = ServiceProvider.GetService<IAuthorizationDataService>();
            Context.Mapper = ServiceProvider.GetService<IMapper>();
            Context.ExpressCodeRepo = ServiceProvider.GetService<IRepository<Olma.ExpressCode>>();
            

            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

            // Assign rules to the Evaluator
            // No validation is needed



            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<ExpressCodesSearchRequest>
        {
            public ContextModel(ExpressCodesSearchRequest parent, IRule rule) : base(parent, rule)
            {

            }

            public RulesBundle Rules { get; protected internal set; }

            public ExpressCodesSearchRequest ExpressCodesSearchRequest { get; set; }

            public IRepository<Olma.ExpressCode> ExpressCodeRepo { get; protected internal set; }
            
            public IMapper Mapper { get; protected internal set; }
            public IAuthorizationDataService AuthData { get; protected internal set; }

            public IQueryable<Olma.ExpressCode> ExpressCodeSearchQuery => BuildExpressCodeSearchQuery();
            

            private IQueryable<Olma.ExpressCode> BuildExpressCodeSearchQuery()
            {
                #region Security

                var customerIds = AuthData.GetCustomerIds();
                var query = ExpressCodeRepo.FindByCondition(e => customerIds.Contains(e.IssuingCustomer.Id)).AsNoTracking();

                #endregion

                #region Apply Filter

                if (!string.IsNullOrEmpty(ExpressCodesSearchRequest.ExpressCode))
                {
                    var expressCode = ExpressCodesSearchRequest.ExpressCode;
                    query = query.Where(e => e.DigitalCode.Contains(expressCode));
                }

                if (!string.IsNullOrEmpty(ExpressCodesSearchRequest.IssuingDivisionName))
                {
                    var issuingDivisionName = ExpressCodesSearchRequest.IssuingDivisionName;
                    query = query.Where(e => e.IssuingDivision.Name == issuingDivisionName);
                }
                #endregion

                #region Ordering
                query = query.OrderBy(e => e.DigitalCode);
                #endregion

                return query;
            }
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