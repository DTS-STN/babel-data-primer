
using System.Collections.Generic;
using System.Linq;
using DataPrimer.Models;

namespace DataPrimer.Storage
{
    public class SimulationEFStore : IStoreData
    {  
        private readonly AppDbContext _context;

        public SimulationEFStore(AppDbContext context)
        {
            _context = context;
        }
        
        public void Store(List<Persons> persons) {
            foreach (var person in persons) {
                _context.Add(person);
            }

            _context.SaveChanges();
        }
    }
}