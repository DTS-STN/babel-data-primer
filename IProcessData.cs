using System.Collections.Generic;

using DataPrimer.Models;
using DataPrimer.Simulation;

namespace DataPrimer
{
    public interface IProcessData
    {
        Persons Process(ProcessedApplication processedApplication);
    }
}