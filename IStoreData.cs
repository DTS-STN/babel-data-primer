using System.Collections.Generic;

using esdc_simulation_classes.MaternityBenefits;

namespace DataPrimer
{
    // Interface for the Storage Step
    public interface IStoreData
    {
         void Store(List<MaternityBenefitsPersonRequest> persons);
    }
}