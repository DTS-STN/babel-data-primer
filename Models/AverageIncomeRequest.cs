using System;

namespace DataPrimer.Models
{
    public class AverageIncomeRequest
    {
        public Roe Roe { get; set; }
        public DateTime ApplicationDate { get; set;}
        public int NumBestWeeks { get; set; }
    }
}