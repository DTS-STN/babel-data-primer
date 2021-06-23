using System;
using System.Collections.Generic;

namespace DataPrimer.Models.Raw
{
    public class RawRoe
    {
        public string PayPeriodType { get; set; } // enum?
        public DateTime LastDayForWhichPaid { get; set; }
        public DateTime FinalPayPeriodDay { get; set; }
        public DateTime FirstDayForWhichPaid { get; set; }
        public List<RawPayPeriod> PayPeriods { get; set; }
    }
}