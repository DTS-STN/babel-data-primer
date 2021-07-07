using esdc_rules_classes.BestWeeks;
using esdc_rules_classes.AverageIncome;

namespace DataPrimer.Rules
{
    public interface IRulesEngine
    {
        int GetBestWeeks(BestWeeksRequest req);
        decimal GetAverageIncome(AverageIncomeRequest req);
    }
}