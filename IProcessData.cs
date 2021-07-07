using System.Collections.Generic;

using DataPrimer.Models;
using DataPrimer.Simulation;

namespace DataPrimer
{
    public interface IProcessData
    {
        Person Process(ProcessedApplication processedApplication);
    }
}