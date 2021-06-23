using System.Collections.Generic;

using DataPrimer.Models;
using DataPrimer.Storage;

namespace DataPrimer
{
    public interface IProcessData
    {
        Persons Process(ProcessedApplication processedApplication);
    }
}