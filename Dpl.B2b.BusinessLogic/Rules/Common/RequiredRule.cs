using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq.Expressions;
using DevExpress.CodeParser;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Dpl.B2b.BusinessLogic.Rules.Common
{
    public class RequiredRule : BaseValidationRule, IParentChildRule
    {
        private ILocalizableMessage Message { get; } = new ResourceValid();
        
        public object Context
        {
            get; set;
        }

        public RequiredRule()
        {
            ResourceName = nameof(RequiredRule);
        }
        
        public RequiredRule(IRule parentRule): this()
        {
            ParentRule = parentRule;
        }

        public IRule ParentRule { get; }
        
        public override void Evaluate()
        {
            if(Context==null) return;
            var contextType = Context.GetType();
            
            var properties = contextType.GetProperties();

            var requiredAttributes = properties.ToDictionary(
                p => p.Name,
                p => p.GetCustomAttributes(typeof(RequiredAttribute), false).OfType<RequiredAttribute>());

            var filter = requiredAttributes.Where(p => p.Value.Any());
            
            foreach (var (key, value) in filter)
            {
                var attr = value.FirstOrDefault();
                if (attr==null) continue;
                
                RuleState.AddMessage(!attr.IsValid(Context), $"{contextType.Name}_{key}, " + "_", Message);
            }
        }
    }
}