using System.Collections.Generic;

using esdc_simulation_classes.MaternityBenefits;

namespace DataPrimer
{
    public interface IStoreData
    {
         void Store(List<MaternityBenefitsPersonRequest> persons);
    }
}