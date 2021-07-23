using System; 

using esdc_rules_classes.AverageIncome;

namespace DataPrimer.Models
{
    // This class is the result of the "Fetching" Step
    // Once the data is retrieved from the DB, it is converted into this form
    // It is then sent the "Processing" Step
    public class ApplicationToProcess
    {
        public ApplicationPerson Person { get; set; }
        public SimpleRoe Roe { get; set; }
        public DateTime ApplicationDate { get; set; }
    }
}