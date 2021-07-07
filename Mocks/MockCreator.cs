using System;
using System.Collections.Generic;
using DataPrimer.Models;

using esdc_rules_classes.AverageIncome;

namespace DataPrimer.Mocks
{
    public static class MockCreator
    {
        // Maybe need a date range?

        public static ProcessedApplication CreateFakeApplication() {
            var ppType = "monthly";

            var applicationDate = GenerateApplicationDate();

            var roe = GenerateRoe(ppType, applicationDate);

            return new ProcessedApplication() {
                Roe = roe,
                ApplicationDate = applicationDate,
                Person = new ApplicationPerson() {
                    PostalCode = GeneratePostalCode(),
                    //Flsah = GenerateFlsah(),
                    Age = GenerateAge(),
                }
            };
        }

        private static DateTime GenerateApplicationDate() {
            // Get previous sunday
            var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var sunday = today.Subtract(new TimeSpan((int)today.DayOfWeek, 0, 0, 0));
            return sunday;
        }

        private static SimpleRoe GenerateMonthlyRoe(DateTime applicationDate) {
            var ppType = GeneratePPType();
            var rnd = new Random();
            
            var payPeriods = GeneratePayPeriods(ppType);

            var finalMonth = rnd.Next(1, 12);
            var finalDay = rnd.Next(1, 28);

            var firstMonth = rnd.Next(1, 12);
            var firstDay = rnd.Next(1, 28);

            var lastDayInMonth = DateTime.DaysInMonth(2021, finalMonth);
            
            return new SimpleRoe() {
                PayPeriodType = ppType,
                PayPeriods = payPeriods,
                FinalPayPeriodDay = new DateTime(DateTime.Now.Year, finalMonth, lastDayInMonth),
                LastDayForWhichPaid = new DateTime(DateTime.Now.Year, finalMonth, finalDay),
                FirstDayForWhichPaid = new DateTime(DateTime.Now.Year - 1, firstMonth, firstDay),
            };
        }

        private static SimpleRoe GenerateRoe(string ppType, DateTime applicationDate) {
            if (ppType == "monthly") {
                return GenerateMonthlyRoe(applicationDate);
            }
            // TODO: Implement other pp types
            throw new Exception("Only Monthly allowed right now");
        }

        private static List<PayPeriod> GeneratePayPeriods(string type) {
            var result = new List<PayPeriod>();
            // Monthly
            int min = 1400;
            int max = 4000;

            int numPeriods = 13;

            var rnd = new Random();
            
            for (int i = 0; i < numPeriods; i++) {
                var nextPP = new PayPeriod(i+1, rnd.Next(min, max));
                result.Add(nextPP);
            }

            return result;
        }

        private static string GeneratePPType() {
            return "monthly";
        }

        private static string GeneratePostalCode() {
            return "A1A 1A1";
        }

        private static int GenerateAge() {
            var rnd = new Random();
            return rnd.Next(16, 36);
        }

        private static string GenerateFlsah() {
            return "English";
        }
    }
}