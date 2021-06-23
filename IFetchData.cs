using System;
using System.Collections.Generic;

using DataPrimer.Models;

namespace DataPrimer
{
    public interface IFetchData
    {
         List<ProcessedApplication> FetchApplications();
    }
}