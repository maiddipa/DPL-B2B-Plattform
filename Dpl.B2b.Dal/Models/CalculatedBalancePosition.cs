using System;

namespace Dpl.B2b.Dal.Models
{
    public class CalculatedBalancePosition
    {
        public int Id { get; set; }

        public int LoadCarrierId { get; set; }
        public virtual LoadCarrier LoadCarrier { get; set; }

        /// <summary>
        ///     Sum of all other balances (CoordinatedBalance + ProvisionalCharge + ProvisionalCredit + UncoordinatedCharge +
        ///     UncoordinatedCredit + BookingRequestBalanceCredit + BookingRequestBalanceCharge
        /// </summary>
        public int ProvisionalBalance { get; set; }

        /// <summary>
        ///     Customer confirmed balance (customer has confirmed that all transactions that count into this balance are approved
        ///     by him)
        /// </summary>
        public int CoordinatedBalance { get; set; }

        /// <summary>
        ///     Belastungen die von DPL noch nicht zu ende bearbeitet wurden
        ///     Kunde will Paletten, DPL stellt Lieferschein aus aber quittierter Lieferschein mit etwaigen änderungen der mengen
        ///     ist noch nicht wieder bei DPL
        /// </summary>
        public int ProvisionalCharge { get; set; }

        /// <summary>
        ///     Gutschriften die von DPL noch nicht zu ende bearbeitet wurden
        ///     Kunde stellt Paletten bereit (quelle von kunde will paletten), DPL stellt Lieferschein aus aber quittierter
        ///     Lieferschein mit etwaigen änderungen der mengen ist noch nicht wieder bei DPL
        /// </summary>
        public int ProvisionalCredit { get; set; }

        public int InCoordinationCharge { get; set; }
        public int InCoordinationCredit { get; set; }

        /// <summary>
        ///     Belastungen die von DPL zu ende bearbeitet wurden aber Kunde hat noch nicht approved (monatlicher bereicht)
        /// </summary>
        public int UncoordinatedCharge { get; set; }

        /// <summary>
        ///     Gutschriften die von DPL zu ende bearbeitet wurden aber Kunde hat noch nicht approved (monatlicher bereicht)
        /// </summary>
        public int UncoordinatedCredit { get; set; }

        public DateTime LatestUncoordinatedCharge { get; set; }
        public DateTime LatestUncoordinatedCredit { get; set; }


        // TODO We need to discuss if its neccessary to be able to seperate out these online only balances in the UI
        public int PostingRequestBalanceCredit { get; set; }
        public int PostingRequestBalanceCharge { get; set; }
    }
}