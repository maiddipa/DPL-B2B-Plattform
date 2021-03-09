using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Extensions;
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

namespace Dpl.B2b.BusinessLogic.Rules.Documents.Search
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>
    {
        public MainRule(DocumentSearchRequest request, IRule parentRule = null)
        {
            // Create Context
            Context = new ContextModel(request, this);
            ParentRule = parentRule;

            ValidEvaluate += OnValidEvaluate;
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

            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

            // Assign rules to the Evaluator
            // No need to validate deeper
            
            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();
            
            // Add all Message from ruleResult
            MergeFromResult(ruleResult);
            
            // Add Message if ruleResult is not success
            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
        }

        private void OnValidEvaluate(object sender, EvaluateInternalEventArgs evaluateInternalEventArgs)
        {
            var request = Context.Parent;

            var documentRepo = ServiceProvider.GetService<IRepository<Olma.Document>>();
            var mapper = ServiceProvider.GetService<IMapper>();
            
            var query = documentRepo
                .FindAll()
                .AsNoTracking();

            #region convert search request

            #endregion

            #region ordering

            query = query.OrderByDescending(d => d.IssuedDateTime);

            #endregion

          
            var projectedQuery = query.ProjectTo<Document>(mapper.ConfigurationProvider);
            
            Context.DocumentPagination=projectedQuery.ToPaginationResult(request);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<DocumentSearchRequest>
        {
            public ContextModel(DocumentSearchRequest parent, MainRule rule) : base(parent, rule)
            {

            }

            public RulesBundle Rules { get; protected internal set; }
            public IPaginationResult<Document> DocumentPagination { get; set; }
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