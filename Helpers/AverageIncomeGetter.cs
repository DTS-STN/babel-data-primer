using DataPrimer.Helpers;
using DataPrimer.Models;
using DataPrimer.Rules;

namespace DataPrimer.Helpers
{
    public class AverageIncomeGetter : IGetAverageIncome
    {
        private readonly IRulesEngine _rulesEngine;

        public AverageIncomeGetter(IRulesEngine rulesEngine) {
            _rulesEngine = rulesEngine;
        }

        public decimal Get(AverageIncomeRequest req) {
            return _rulesEngine.GetAverageIncome(req);
        }
    }
}