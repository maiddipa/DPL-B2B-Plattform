using System;
using System.Linq;
//using System.Data.Entity;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Warnings.Common;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.Documents.GetDocumentDownload
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>
    {
        public MainRule((int id, DocumentFileType type) request, IRule parentRule = null)
        {
            // Create Context
            Context = new ContextModel(request, this);
            ParentRule = parentRule;

            ValidEvaluate += OnValidEvaluate;
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

            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

            // Assign rules to the Evaluator
            rulesEvaluator.Eval(Context.Rules.DocumentResource);
                        
            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();
            
            // Add all Message from ruleResult
            MergeFromResult(ruleResult);
            
            // Add Message if ruleResult is not success
            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
        }

        private async void OnValidEvaluate(object sender, EvaluateInternalEventArgs args)
        {
            var storage = ServiceProvider.GetService<IStorageService>() ;
            var document = Context.Document;
            var fileType = Context.DocumentFileType;

            if(document.StateId == 255 || (document.Voucher != null && document.Voucher.Status == VoucherStatus.Canceled))
            {
                fileType = DocumentFileType.Cancellation;
                // Fallback if cancellation document does not exist
                if(!document.Files.Any(x => x.FileType == DocumentFileType.Cancellation))
                {
                    fileType = DocumentFileType.Copy;
                }
            }
            else if(fileType == DocumentFileType.Copy
               && document.CreatedAt?.AddMinutes(document.Type.OriginalAvailableForMinutes) >= DateTime.UtcNow)
            {
                fileType = DocumentFileType.Composite;
            }

            var fileName = document.Files.Where(i => i.FileType == fileType)
                .Select(i => i.File.InternalFullPathAndName).SingleOrDefault();
           
            Context.DownloadLink = await storage.GetDownloadLink(fileName);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<(int id, DocumentFileType type)>
        {
            public ContextModel((int id, DocumentFileType type) parent, MainRule rule) : base(parent, rule)
            {

            }

            public RulesBundle Rules { get; protected internal set; }
            public int DocumentId => Parent.id;
            public DocumentFileType DocumentFileType => Parent.type;
            public Olma.Document Document => Rules.DocumentResource.Context.Resource;
            public string DownloadLink { get; protected internal set; }
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, IRule rule)
            {
                DocumentResource = new ResourceRule<Olma.Document>(context.DocumentId, rule)
                    .AsNoTracking()
                    .UseEagleLoad(i => i
                        .Include(e => e.Type)
                        .Include(e => e.Voucher)
                        .Include(e => e.Files)
                        .ThenInclude(e => e.File));

            }

            public readonly ResourceRule<Olma.Document> DocumentResource;
        }

        #endregion
    }
}