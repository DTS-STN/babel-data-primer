using System.Collections.Generic;
using DataPrimer.Simulation;

namespace DataPrimer
{
    public interface IStoreData
    {
         void Store(List<Person> persons);
    }
}