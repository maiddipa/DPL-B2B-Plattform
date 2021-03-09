using System;
using System.Linq;

namespace Dpl.B2b.BusinessLogic.Rules.User.Shared
{
    /// <summary>
    /// Einfache Regel zum erzeugen eines zufälligen Passworts
    /// </summary>
    public class RandomPasswordRule:IRule<string>
    {
        private const string CharsPrefix = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const string CharsPostfix = "0123456789!*,.%";
        
        public RandomPasswordRule(int prefixLenght = 8, int postfixLenght = 3)
        {
            this.prefixLenght = prefixLenght;
            this.postfixLenght = postfixLenght;
        }

        /// <summary>
        /// Länge des vorderen Teil des Passworts
        /// </summary>
        private readonly int prefixLenght;
        
        /// <summary>
        /// Länge des hinteren Teil des Passworts
        /// </summary>
        private readonly int postfixLenght;

        /// <summary>
        /// Neu erstelltes Passwort
        /// </summary>
        public string NewPassword { get; private set; }
        
        void IRule.Evaluate()
        {
            NewPassword = IsMatch() 
                ? RandomString(prefixLenght, postfixLenght) 
                : string.Empty;
        }

        string IRule<string>.Evaluate()
        {
            ((IRule)this).Evaluate();
            return NewPassword;
        }
        
        public static string RandomString(int prefixLenght, int postfixLenght)
        {
            var random = new Random();

            var prefix= new string(Enumerable.Repeat(CharsPrefix, prefixLenght)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            var postfix= new string(Enumerable.Repeat(CharsPostfix, postfixLenght)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            
            return $"{prefix}{postfix}";
        }

        public bool IsMatch()
        {
            return true;
        }

        public string RuleName { get; }= nameof(RandomPasswordRule);
    }
}