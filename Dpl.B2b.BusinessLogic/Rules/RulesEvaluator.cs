using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Dpl.B2b.BusinessLogic.Rules.Common.Logic;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;


namespace Dpl.B2b.BusinessLogic.Rules
{
    public class RulesEvaluator
    {
        public RulesEvaluator()
        {
            
        }

        public RulesEvaluatorConfig Config { get; } = new RulesEvaluatorConfig();

        private List<IRule> Rules { get; } = new List<IRule>();

        public IRuleWithStateResult Result { get; private set; } = new RuleWithStateResult();

        public RulesEvaluator Eval(IRule rule)
        {
            Rules.Add(rule ?? new InvalidRule());
            
            return this;
        }
        
        public RulesEvaluator EvalMany(IEnumerable<IRule> rules)
        {
            var rulesList = rules.ToList();
            var filteredRules = rulesList.Where(r => r != null);
            
            if(rulesList.Contains(null)) 
                Rules.Add(new InvalidRule());
            
            Rules.AddRange(filteredRules);
            
            return this;
        }

        public RulesEvaluator EvalIf(IRule rule, bool condition)
        {
            if (condition)
            {
                Rules.Add(rule ?? new InvalidRule());
            }
            return this;
        }
        
        public RulesEvaluator EvalIf<T>(IRule rule, T input, Func<T, bool> condition)
        {
            return EvalIf(rule, condition(input));
        }
        
        private static bool EvalIf(Expression<Func<bool>> expr)
        {
            return expr.Compile().Invoke();
        }

        public IRuleResult Evaluate()
        {
            var result = new RuleWithStateResult();
            var rules = Rules
                .Where(r => r != null)
                .Where(r => r.IsMatch());

            foreach (var rule in rules)
            {
                rule.Evaluate();

                switch (rule)
                {
                    // Regel besitzt einen internen State indem Informationen gespeichert sind
                    case IRuleWithState ruleWithState:
                        result.State.Merge(ruleWithState.RuleState);
                        break;
                   
                    // Regel besitzt Interface zum validieren
                    case IValidationRule validationRule:
                    {
                        var isValid = validationRule.IsValid();
                        if (!isValid)
                        {
                            var type = validationRule.GetType();
                            var name = type.Name;
                            result.State.Add(name, new RuleState {RuleStateItems = {new NotAllowedByRule()}});
                        }

                        break;
                    }

                    // Regel ist als logisches Prädikat zu verstehen dessen wahrheit im Rückgabewert geliefert wird.
                    case IRule<bool> concreteRule:
                    {
                        var eval = concreteRule.Evaluate();
                        if (!eval)
                        {
                            var type = concreteRule.GetType();
                            var name = type.Name;
                            result.State.Add(name, new RuleState {RuleStateItems = {new NotAllowedByRule()}});
                        }

                        break;
                    }
                    default:
                        // Rule has no effect on result
                        break;
                    
                }

                // Falls alle Regeln überprüft werden sollen die Schleife weiter durchlaufen
                if (Config.CheckAllRules) continue;

                // Andernfalls bei erster fehlerhaften Regel Ergebnis zurückliefern
                if (!result.IsSuccess)
                {
                    Result = result;
                    return Result;
                }
            }

            Result = result;
            return Result;
        }

        public static RulesEvaluator Create()
        {
            return new RulesEvaluator();
        }

        public RulesEvaluator StopEvaluateOnFirstInvalidRule()
        {
            Config.CheckAllRules = false;
            return this;
        }

        public RulesEvaluator CheckAllRules()
        {
            Config.CheckAllRules = true;
            return this;
        }

        public class RulesEvaluatorConfig
        {
            public bool CheckAllRules { get; set; } = false;
        }
    }

  
}