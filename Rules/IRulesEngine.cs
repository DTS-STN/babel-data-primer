using DataPrimer.Models;

namespace DataPrimer.Rules
{
    public interface IRulesEngine
    {
        int GetBestWeeks(BestWeeksRequest req);
        decimal GetAverageIncome(AverageIncomeRequest req);
    }
}