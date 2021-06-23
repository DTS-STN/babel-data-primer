using System; 

namespace DataPrimer.Models
{
    public class ProcessedApplication
    {
        public ApplicationPerson Person { get; set; }
        public Roe Roe { get; set; }
        public DateTime ApplicationDate { get; set; }

    }
}