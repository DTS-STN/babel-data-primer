using esdc_rules_classes.BestWeeks;
using esdc_rules_classes.AverageIncome;

namespace DataPrimer.Rules
{
    public class RulesEngine : IRulesEngine
    {  
        private readonly RulesApi _api;

        public RulesEngine(RulesApi api) {
            _api = api;
        }

        public int GetBestWeeks(BestWeeksRequest req) {
            var result = _api.Execute<BestWeeksResponse>("BestWeeks", req, "numBestWeeks");
            return result.NumBestWeeks;
        }

        public decimal GetAverageIncome(AverageIncomeRequest req) {
            var result = _api.Execute<AverageIncomeResponse>("AverageIncome", req, "averageIncome");
            return result.AverageIncome;
        }
    }
}