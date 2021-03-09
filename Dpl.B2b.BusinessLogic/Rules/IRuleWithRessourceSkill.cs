namespace Dpl.B2b.BusinessLogic.Rules
{
    interface IRuleWithRessourceSkill: IValidationRule, IRuleWithServiceProvider
    {
        IValidationRule ResourceRule { get; }
    }
}