using System.Collections.Generic;

using DataPrimer.Models;
using DataPrimer.Simulation;

using esdc_simulation_classes.MaternityBenefits;

namespace DataPrimer
{
    public interface IProcessData
    {
        MaternityBenefitsPersonRequest Process(ProcessedApplication processedApplication);
    }
}