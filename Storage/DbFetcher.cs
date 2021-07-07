using System;
using System.Collections.Generic;
using System.Linq;

using DataPrimer.Models;
using DataPrimer.Mocks;
using esdc_rules_classes.AverageIncome;

namespace DataPrimer.Storage
{
    public class DbFetcher : IFetchData
    {
        private readonly int _amount;
        private readonly BabeldbContext _context;
        public DbFetcher(BabeldbContext context, int amount) {
            _amount = amount;
            _context = context;
        }
        public List<ProcessedApplication> FetchApplications() {
            var result = new List<ProcessedApplication>();
            var apps = _context.CliRoe.ToList();
            var allEarnings = _context.Earnings.ToList();

            var appSubset = apps.Take(_amount);

            foreach (var app in appSubset) {
                var earnings = allEarnings.Where(d => d.RoeId == app.RoeId);

                var nextPerson = new ApplicationPerson() {
                    Province = app.Province,
                    EducationLevel = app.EducationLevel,
                    LanguageSpoken = app.LanguageSpoken,
                    Age = DateTime.Now.Year - app.YearOfBirth,
                    PostalCode = app.PostalCode
                };

                var nextEarnings = new List<PayPeriod>();
                foreach (var earning in earnings) {
                    var amount = Convert.ToDecimal(earning.InsurableEarningAmt);
                    var num = Convert.ToInt32(earning.PayPeriodNbr);
                    var nextPP = new PayPeriod(num, amount);
                    nextEarnings.Add(nextPP);
                }

                var nextRoe = new SimpleRoe() {
                    PayPeriodType = app.PayPeriodType.ToLower(),
                    LastDayForWhichPaid = app.LastDayPaidDate,
                    FinalPayPeriodDay = app.FinalPayPeriodEndDate,
                    FirstDayForWhichPaid = app.FirstDayWorkedDate,
                    PayPeriods = nextEarnings
                };

                var appDate = app.ApplicationStartedDate;

                var nextRes = new ProcessedApplication() {
                    Roe = nextRoe,
                    Person = nextPerson,
                    ApplicationDate = appDate
                };

                result.Add(nextRes);
            }

            return result;
        }
    }
}