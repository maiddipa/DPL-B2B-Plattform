using System.Linq;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Order;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.Order.Cancel
{
    public class OrderCancelStatusValidationRule<TResource, TContext> : BaseValidationWithServiceProviderRule<
        OrderCancelStatusValidationRule<TResource, TContext>,
        OrderCancelStatusValidationRule<TResource, TContext>.ContextModel> where TResource : class
    {
        public OrderCancelStatusValidationRule(GetOperatorRule<TContext, TResource> getOp, IRule rule)
        {
            Context = new ContextModel(getOp, this);
            ParentRule = rule;
        }

        /// <summary>
        ///     Message for RuleState if Rule is invalid
        /// </summary>
        protected override ILocalizableMessage Message => new OrderCancelStatusNotValid();

        /// <summary>
        ///     Internal Method for Evaluate
        /// </summary>
        protected override void EvaluateInternal()
        {
            var ressource = Context.Parent.Evaluate();
            var order = ressource as Olma.Order;
            var authData = ServiceProvider.GetService<IAuthorizationDataService>();

            var cancellableStates = new[]
                {OrderStatus.Pending, OrderStatus.Confirmed, OrderStatus.PartiallyMatched, OrderStatus.Matched};
            if (authData.GetUserRole() == UserRole.DplEmployee)
                cancellableStates = cancellableStates.Append(OrderStatus.CancellationRequested).ToArray();

            AddMessage(order != null && !cancellableStates.Contains(order.Status), ResourceName, Message);
        }

        #region Internal

        /// <summary>
        ///     Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<GetOperatorRule<TContext, TResource>>
        {
            public ContextModel(GetOperatorRule<TContext, TResource> parent, IRule rule) : base(parent, rule)
            {
            }
        }

        #endregion
    }
}