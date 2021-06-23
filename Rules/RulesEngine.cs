using DataPrimer.Models;

namespace DataPrimer.Rules
{
    public class RulesEngine : IRulesEngine
    {  
        private readonly RulesApi _api;

        public RulesEngine(RulesApi api) {
            _api = api;
        }

        public int GetBestWeeks(BestWeeksRequest req) {
            var result = _api.Execute<int>("BestWeeks", req, "numBestWeeks");
            return result;
        }

        public decimal GetAverageIncome(AverageIncomeRequest req) {
            var result = _api.Execute<int>("AverageIncome", req, "averageIncome");
            return result;
        }
    }
}