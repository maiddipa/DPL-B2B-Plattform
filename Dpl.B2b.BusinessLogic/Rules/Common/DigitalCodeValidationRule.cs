using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.ServiceModel.Channels;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.ExpressCode;
using Dpl.B2b.Contracts.Models;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.Common
{
    public class DigitalCodeValidationRule : BaseValidationWithServiceProviderRule<DigitalCodeValidationRule, DigitalCodeValidationRule.ContextModel>
    {
        protected override ILocalizableMessage Message { get; } = new DigitalCodeNotExistOrCanceled();
        
        public DigitalCodeValidationRule(string digitalCodeNumber, IRule parentRule)
        {
            ResourceName = "DigitalCode";
            Context = new ContextModel(digitalCodeNumber, this);
            ParentRule = parentRule;
        }

        /// <summary>
        /// Load digitalCodeNumber if is not null or empty
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DigitalCodeValidationRule LoadDigitalCodeNumber()
        {
            // this rule need service scope
            if(ServiceProvider==null)
                throw new Exception("First set a service provider");
            
            // avoid unnecessary attempts to load
            if (string.IsNullOrEmpty(Context.DigitalCodeNumber)) return this;
            
            // DI get repository 
            var repository = ServiceProvider.GetService<IRepository<Olma.ExpressCode>>();

            // load expressCode
            Context.ExpressCode = repository.FindByCondition(e => e.DigitalCode == Context.DigitalCodeNumber)
                .Include(i => i.PostingAccountPreset)
                .SingleOrDefault();

            return this;
        }

        protected override void EvaluateInternal()
        {
            if (Context.ExpressCode == null || Context.ExpressCode.IsCanceled)
                RuleState.Add(ResourceName, new RuleState(Context.ExpressCode) {RuleStateItems = {Message}});
        }
        
        public class ContextModel: ContextModelBase<string>
        {
            public ContextModel(string digitalCodeNumber, [NotNull] IRule rule): base(digitalCodeNumber, rule)
            {
                
            }

            public string DigitalCodeNumber => Parent;
            public Olma.ExpressCode ExpressCode { get; set; }
        }
    }
}