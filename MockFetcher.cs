using System;
using System.Collections.Generic;

using DataPrimer.Models;
using DataPrimer.Mocks;

namespace DataPrimer
{
    public class MockFetcher : IFetchData
    {
        private readonly int _amount;
        public MockFetcher(int amount) {
            _amount = amount;
        }
        public List<ProcessedApplication> FetchApplications() {
            var result = new List<ProcessedApplication>();
            
            for (int i = 0; i < _amount; i++) {
                var nextApplication = MockCreator.CreateFakeApplication();
                result.Add(nextApplication);
            }

            return result;
        }
    }
}