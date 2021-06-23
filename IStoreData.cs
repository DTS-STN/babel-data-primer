using System.Collections.Generic;

using DataPrimer.Storage;

namespace DataPrimer
{
    public interface IStoreData
    {
         void Store(List<Persons> persons);
    }
}