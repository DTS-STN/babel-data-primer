using System.Collections.Generic;

using DataPrimer.Models;
using DataPrimer.Simulation;

using esdc_simulation_classes.MaternityBenefits;

namespace DataPrimer
{
    // Interface for the Processing Step
    public interface IProcessData
    {
        MaternityBenefitsPersonRequest Process(ApplicationToProcess application);
    }
}