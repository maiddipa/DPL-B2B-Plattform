using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.OrderGroup;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.OrderGroup.Create
{
    public class LoadingLocationIdValidationRule : BaseValidationWithServiceProviderRule<LoadingLocationIdValidationRule,
        LoadingLocationIdValidationRule.ContextModel>
    {
        public LoadingLocationIdValidationRule(int? loadingLocationId, IRule parentRule)
        {
            ResourceName = "LoadingLocation";
            Context = new ContextModel(loadingLocationId, this);
            ParentRule = parentRule;
        }

        protected override ILocalizableMessage Message => new LoadingLocationNotFound();

        protected override void EvaluateInternal()
        {
            switch (Context.LoadingLocationId)
            {
                case null:
                    return;
                case 0:
                    RuleState.Add(ResourceName, new RuleState {RuleStateItems = {new LoadingLocationRequired()}});
                    return;
            }

            var loadingLocationRepository = ServiceProvider.GetService<IRepository<Olma.LoadingLocation>>();
            var loadingLocationExists = loadingLocationRepository.FindAll()
                .Any(location => location.Id == Context.LoadingLocationId);

            AddMessage(!loadingLocationExists, ResourceName, Message);
        }

        public class ContextModel : ContextModelBase<int?>
        {
            public ContextModel(int? loadingLocationId, [NotNull] LoadingLocationIdValidationRule rule) : base(
                loadingLocationId, rule)
            {
            }

            public int? LoadingLocationId => Parent;
        }
    }
}