using DataPrimer.Models;

namespace DataPrimer.Helpers
{
    public interface IGetBestWeeks
    {
         int Get(BestWeeksRequest req);
    }
}