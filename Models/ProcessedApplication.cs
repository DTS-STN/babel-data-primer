using System; 

using esdc_rules_classes.AverageIncome;

namespace DataPrimer.Models
{
    public class ProcessedApplication
    {
        public ApplicationPerson Person { get; set; }
        public SimpleRoe Roe { get; set; }
        public DateTime ApplicationDate { get; set; }

    }
}