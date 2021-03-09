using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.ExpressCode
{
    public class DigitalCodeIssuingCustomerDoesNotMatch : ErrorBase
    {
        public override string Meaning => "DigitalCodeIssuingCustomerDoesNotMatch";
        public override string Description => "DigitalCode is not issued by your Company";
    }
}
