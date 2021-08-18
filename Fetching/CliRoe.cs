using System;

namespace DataPrimer.Fetching
{
    public partial class CliRoe
    {
        public int RoeId { get; set; }
        public int ClientId { get; set; }
        public DateTime ApplicationStartedDate { get; set; }
        public string PersonUid { get; set; }
        public int YearOfBirth { get; set; }
        public int GenderId { get; set; }
        public string Gender { get; set; }
        public int EducationLevelId { get; set; }
        public string EducationLevel { get; set; }
        public int LanguageId { get; set; }
        public string LanguageSpoken { get; set; }
        public string Municipality { get; set; }
        public string PostalCode { get; set; }
        public string Province { get; set; }
        public int PayPeriodTypeId { get; set; }
        public string PayPeriodType { get; set; }
        public DateTime FinalPayPeriodEndDate { get; set; }
        public DateTime FirstDayWorkedDate { get; set; }
        public DateTime LastDayPaidDate { get; set; }
        public double TotalInsurableEarningAmt { get; set; }
        public int TotalInsurableHourNbr { get; set; }
    }
}
