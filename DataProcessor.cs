using System;
using DataPrimer.Models;
using DataPrimer.Helpers;
using DataPrimer.Simulation;
using DataPrimer.Rules;

using esdc_rules_classes.BestWeeks;
using esdc_rules_classes.AverageIncome;
using esdc_simulation_classes.MaternityBenefits;
using esdc_simulation_classes;


namespace DataPrimer
{
    public class DataProcessor : IProcessData
    {
        private readonly IRulesEngine _rules;

        public DataProcessor(IRulesEngine rules) {
            _rules = rules;
        }
        
        public Person Process(ProcessedApplication processedApplication) {
            var bestWeeksRequest = new BestWeeksRequest() {
                PostalCode = processedApplication.Person.PostalCode
            };
            var numBestWeeks = _rules.GetBestWeeks(bestWeeksRequest);

            var averageIncomeRequest = new AverageIncomeRequest() {
                Roe = processedApplication.Roe,
                ApplicationDate = processedApplication.ApplicationDate,
                NumBestWeeks = numBestWeeks
            };
            var averageIncome = _rules.GetAverageIncome(averageIncomeRequest);

            var result = new Person() {
                Age = processedApplication.Person.Age,
                AverageIncome = averageIncome,
                SpokenLanguage = processedApplication.Person.LanguageSpoken,
                EducationLevel = processedApplication.Person.EducationLevel,
                Province = processedApplication.Person.Province
            };

            return result;
        }

    }
}

