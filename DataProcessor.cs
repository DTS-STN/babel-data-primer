using System;
using DataPrimer.Models;
using DataPrimer.Helpers;
using DataPrimer.Storage;

namespace DataPrimer
{
    public class DataProcessor : IProcessData
    {
        private readonly IGetBestWeeks _bestWeeksGetter;
        private readonly IGetAverageIncome _averageIncomeGetter;

        public DataProcessor(IGetBestWeeks bestWeeksGetter, IGetAverageIncome averageIncomeGetter) {
            _bestWeeksGetter = bestWeeksGetter;
            _averageIncomeGetter = averageIncomeGetter;
        }
        
        public Persons Process(ProcessedApplication processedApplication) {
            var bestWeeksRequest = new BestWeeksRequest() {
                PostalCode = processedApplication.Person.PostalCode
            };
            var numBestWeeks = _bestWeeksGetter.Get(bestWeeksRequest);

            var averageIncomeRequest = new AverageIncomeRequest() {
                Roe = processedApplication.Roe,
                ApplicationDate = processedApplication.ApplicationDate,
                NumBestWeeks = numBestWeeks
            };
            var averageIncome = _averageIncomeGetter.Get(averageIncomeRequest);

            var result = new Persons() {
                Id = Guid.NewGuid(),
                Age = processedApplication.Person.Age,
                Flsah = processedApplication.Person.Flsah,
                AverageIncome = averageIncome
            };

            return result;
        }
    }
}

