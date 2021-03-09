using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Olma = Dpl.B2b.Dal.Models;



namespace Dpl.B2b.BusinessLogic.Rules.Vouchers.Create
{
    public class VoucherRule : BaseValidationWithServiceProviderRule<VoucherRule, VoucherRule.ContextModel>
    {
        public VoucherRule(MainRule.ContextModel request, IRule parentRule)
        {
            // Create Context
            Context = new ContextModel(request, this);
            ParentRule = parentRule;
        }

        /// <summary>
        /// Message for RuleState if Rule is invalid 
        /// </summary>
        protected override ILocalizableMessage Message => new NotAllowedByRule();

        /// <summary>
        /// Internal Method for Evaluate
        /// </summary>
        protected override void EvaluateInternal()
        {
            // Initialized all rules. Instancing only takes place during execution, since otherwise the service scope has not yet been assigned
            Context.Rules = new RulesBundle(Context, this);
            var mapper = ServiceProvider.GetService<IMapper>();
            
            Context.Voucher = mapper.Map<Olma.Voucher>(Context.Parent.Parent);
            
            var expressCode = Context.Parent.ExpressCode;
            
            if (!string.IsNullOrEmpty(expressCode))
            {
                var expressCodeRepo = ServiceProvider.GetService<IRepository<Olma.ExpressCode>>();
                
                Context.Voucher.ExpressCodeId = expressCodeRepo
                    .FindByCondition(e => e.DigitalCode == expressCode)
                    .SingleOrDefault()?
                    .Id;
            }

            if (Context.Parent.Type == VoucherType.Direct)
            {
                Context.Voucher.Status = VoucherStatus.Accounted;
            }

            var customerDivisionRepo = ServiceProvider.GetService<IRepository<Olma.CustomerDivision>>();
            var customerDocumentSetting = customerDivisionRepo
                .FindAll().Where(cd => cd.Id == Context.Parent.CustomerDivisionId)
                .Include(cd => cd.Customer).ThenInclude(c => c.DocumentSettings)
                .ThenInclude(ds => ds.DocumentType)
                .SelectMany(cd => cd.Customer.DocumentSettings)
                .SingleOrDefault(ds => ds.DocumentType.Type == Context.Parent.Rules.DocumentType.Context.DocumentType && ds.LoadCarrierTypeId == null);
            Context.Voucher.ValidUntil = customerDocumentSetting?.ValidForMonths != null
                ? new DateTime(DateTime.Today.AddMonths((int) customerDocumentSetting.ValidForMonths).Year, 12, 31) 
                : DateTime.Today.AddMonths(6);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<MainRule.ContextModel>
        {
            public ContextModel(MainRule.ContextModel parent, IRule rule) : base(parent, rule)
            {
                
            }

            public RulesBundle Rules { get; protected internal set; }
            public Olma.Voucher Voucher { get; protected internal set; }
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, IRule rule)
            {

            }
        }

        #endregion
    }
}