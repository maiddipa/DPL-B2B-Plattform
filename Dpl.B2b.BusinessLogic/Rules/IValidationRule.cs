namespace Dpl.B2b.BusinessLogic.Rules
{
    /// <summary>
    /// Class for use within predicate logic
    /// </summary>
    public interface IValidationRule : IRule
    {
        public bool IsValid();

        /// <summary>
        /// Checks predicate
        /// </summary>
        /// <returns>true or false</returns>
        public bool Validate();
    }
}