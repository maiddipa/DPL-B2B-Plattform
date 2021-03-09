using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Rules;

namespace Dpl.B2b.Web.Api
{
    public class DplProblemDetails : ValidationProblemDetails
    {
        public DplProblemDetails()
        {
        }

        public DplProblemDetails(ModelStateDictionary modelState) : base(modelState)
        {
        }

        public RuleState[] RuleStates { get; set; }

        public ServiceState[] ServiceStates { get; set; }
    }
}
