using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.TransportOfferings;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection; //using JetBrains.Annotations;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.TransportOfferings.CreateBid
{
    public class TransportValidRule : BaseValidationWithServiceProviderRule<TransportValidRule,
        TransportValidRule.ContextModel>, IParentChildRule
    {
        public TransportValidRule(MainRule.ContextModel request, IRule parentRule)
        {
            Context = new ContextModel(request, this);
            ParentRule = parentRule;
        }

        protected override ILocalizableMessage Message => new TransportValid();

        protected override void EvaluateInternal()
        {
            Context.Rules = new RulesBundle(Context, this);
            
            var ruleResult = RulesEvaluator.Create()
                .Eval(Context.Rules.TransportIdRequired)
                .Evaluate();
            
            MergeFromResult(ruleResult);
            
            if (!ruleResult.IsSuccess) 
                return;

            var repository = ServiceProvider.GetService<IRepository<Olma.Transport>>();
            
            Context.Transport = repository.FindAll()
                .Where(i => i.Id == Context.TransportId)
                .Include(i => i.Bids)
                .SingleOrDefault();

            var transportNotFound = Context.Transport == null;
            AddMessage(transportNotFound, ResourceName, new TransportNotFound());

            var transportExist = !transportNotFound;
            var requested = Context.Transport?.Status == Olma.TransportStatus.Requested;
            var allocated = Context.Transport?.Status == Olma.TransportStatus.Allocated;
            
            var invalid = transportExist && !(requested || allocated);
            AddMessage(invalid, ResourceName, Message);
        }
        
        #region Internal
        
        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<MainRule.ContextModel>
        {
            public ContextModel(MainRule.ContextModel request, TransportValidRule rule) :
                base(request, rule)
            {
                
            }

            public RulesBundle Rules { get; protected internal set; } 
            
            public int? TransportId => Parent.Parent.TransportId;
            [NotNull] public Olma.Transport Transport { get; protected internal set; } 
        }
        
        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public TransportIdRequiredRule TransportIdRequired { get; }

            public RulesBundle(ContextModel context, IRule rule)
            {
                TransportIdRequired = new TransportIdRequiredRule(context, rule);
            }
        }
        
        #endregion
    }
}