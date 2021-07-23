using System;
using System.Collections.Generic;

using DataPrimer.Models;

namespace DataPrimer
{
    // Interface for the Fetching Step
    public interface IFetchData
    {
         List<ApplicationToProcess> FetchApplications(int maxAmountToFetch);
    }
}