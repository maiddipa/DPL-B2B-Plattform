using System.Diagnostics.CodeAnalysis;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.OrderGroup;

namespace Dpl.B2b.BusinessLogic.Rules.OrderGroup.Create
{
    public class NumberOfLoadsValidationRule : BaseValidationWithServiceProviderRule<NumberOfLoadsValidationRule,
        NumberOfLoadsValidationRule.ContextModel>
    {
        public NumberOfLoadsValidationRule(MainRule.ContextModel context, IRule parentRule)
        {
            ResourceName = "OrderGroup";
            Context = new ContextModel(context, this);
            ParentRule = parentRule;
            MaxNumberOfLoads = 10;
            MinNumberOfLoads = 1;
        }

        protected override ILocalizableMessage Message => new NumberOfLoadsNotValid();
        protected int MaxNumberOfLoads { get; set; }
        protected int MinNumberOfLoads { get; set; }

        public NumberOfLoadsValidationRule SetMax(int maxNumberOfLoads)
        {
            MaxNumberOfLoads = maxNumberOfLoads;
            return this;
        }

        public NumberOfLoadsValidationRule SetMin(int minNumberOfLoads)
        {
            MinNumberOfLoads = minNumberOfLoads;
            return this;
        }

        protected override void EvaluateInternal()
        {
            if (Context.Parent.OrderGroupsCreateRequest.QuantityType == OrderQuantityType.Load &&
                (Context.Parent.OrderGroupsCreateRequest.NumberOfLoads < MinNumberOfLoads ||
                 Context.Parent.OrderGroupsCreateRequest.NumberOfLoads > MaxNumberOfLoads))
                RuleState.Add(ResourceName, new RuleState {RuleStateItems = {Message}});
        }

        public class ContextModel : ContextModelBase<MainRule.ContextModel>
        {
            public ContextModel(MainRule.ContextModel context, [NotNull] NumberOfLoadsValidationRule rule) : base(
                context,
                rule)
            {
            }
        }
    }
}