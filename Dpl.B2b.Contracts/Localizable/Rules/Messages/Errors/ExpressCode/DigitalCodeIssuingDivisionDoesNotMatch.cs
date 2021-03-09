using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.ExpressCode
{
    public class DigitalCodeIssuingDivisionDoesNotMatch : ErrorBase
    {
        public override string Meaning => "DigitalCodeIssuingDivisionDoesNotMatch";
        public override string Description => "DigitalCode is not issued by your division";
    }
}
