using DataPrimer.Models;
using DataPrimer.Models.Raw;

namespace DataPrimer.Helpers
{
    // Note: This is used when fetching raw data from database
    public interface IProcessRawApplications
    {
        ProcessedApplication Process(RawApplication rawApplication);
    }
}