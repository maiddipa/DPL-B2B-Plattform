
namespace Dpl.B2b.BusinessLogic.Rules
{
    public interface IRule
    {
        /// <summary>
        /// Applies knowledge to the context 
        /// </summary>
        public void Evaluate();

        /// <summary>
        /// Knowledge can be applied to the context
        /// </summary>
        public bool IsMatch();
        
        string RuleName { get; }
    }

    public interface IRule<T>: IRule
    {
        /// <summary>
        /// Applies knowledge to the context 
        /// </summary>
        public new T Evaluate();
    }
}