using DataPrimer.Helpers;
using DataPrimer.Models;
using DataPrimer.Rules;

namespace DataPrimer.Helpers
{
    public class BestWeeksGetter : IGetBestWeeks
    {
        private readonly IRulesEngine _rulesEngine;

        public BestWeeksGetter(IRulesEngine rulesEngine) {
            _rulesEngine = rulesEngine;
        }

        public int Get(BestWeeksRequest req) {
            return _rulesEngine.GetBestWeeks(req);
        }
    }
}