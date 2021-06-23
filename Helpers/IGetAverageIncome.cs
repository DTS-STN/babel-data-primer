using DataPrimer.Models;

namespace DataPrimer.Helpers
{
    public interface IGetAverageIncome
    {
        decimal Get(AverageIncomeRequest req);
    }
}