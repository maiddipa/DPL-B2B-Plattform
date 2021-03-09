namespace Dpl.B2b.BusinessLogic.Rules
{
    /// <summary>
    /// If this interface is used then the AuthorizationResourceRule must be the first rule in the Eval.
    /// </summary>
    interface IRuleWithAuthorizationSkill: IValidationRule, IRuleWithServiceProvider
    {
        (IValidationRule AuthorizationRule, IValidationRule ResourceRule) Authorization { get; }
    }
}