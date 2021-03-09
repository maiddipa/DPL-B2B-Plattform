namespace Dpl.B2b.BusinessLogic.Rules
{
    public interface IParentChildRule:IRule
    {
        IRule ParentRule { get; }
    }
}