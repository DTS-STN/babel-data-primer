
using System.Collections.Generic;
using System.Linq;
using DataPrimer.Models;
using DataPrimer.Api;

namespace DataPrimer.Simulation
{
    public class SimulationStore : IStoreData
    {  
        private readonly SimulationApi _api;

        public SimulationStore(SimulationApi api)
        {
            _api = api;
        }
        
        public void Store(List<Person> persons) {
            _api.Execute("Persons", persons);
        }
    }
}