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
        
        public MaternityBenefitsPersonRequest Process(ApplicationToProcess application) {
            var bestWeeksRequest = new BestWeeksRequest() {
                PostalCode = application.Person.PostalCode
            };
            var numBestWeeks = _rules.GetBestWeeks(bestWeeksRequest);

            var averageIncomeRequest = new AverageIncomeRequest() {
                Roe = application.Roe,
                ApplicationDate = application.ApplicationDate,
                NumBestWeeks = numBestWeeks
            };
            var averageIncome = _rules.GetAverageIncome(averageIncomeRequest);

            var result = new MaternityBenefitsPersonRequest() {
                Age = application.Person.Age,
                AverageIncome = averageIncome,
                SpokenLanguage = application.Person.LanguageSpoken,
                EducationLevel = application.Person.EducationLevel,
                Province = application.Person.Province
            };

            return result;
        }

    }
}

