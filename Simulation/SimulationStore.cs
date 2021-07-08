
using System.Collections.Generic;

using esdc_simulation_classes.MaternityBenefits;

namespace DataPrimer.Simulation
{
    public class SimulationStore : IStoreData
    {  
        private readonly SimulationApi _api;

        public SimulationStore(SimulationApi api)
        {
            _api = api;
        }
        
        public void Store(List<MaternityBenefitsPersonRequest> persons) {
            _api.Execute("Persons", persons);
        }
    }
}