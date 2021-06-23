using System;
using System.Collections.Generic;
using System.Linq;
using DataPrimer.Models.Raw;
using DataPrimer.Models;

namespace DataPrimer.Helpers
{
    public class ApplicationProcessor : IProcessRawApplications
    {
        public ProcessedApplication Process(RawApplication rawApplication) {
            
            var payPeriods = rawApplication.Roe.PayPeriods
                .Select(x => new PayPeriod(){
                    Amount = x.PayPeriodAmount,
                    PayPeriodNumber = x.PayPeriodNumber
                }).ToList();

            int age = 0;  
            var dob = rawApplication.ClientData.DateOfBirth;
            age = DateTime.Now.Year - dob.Year;  
            if (DateTime.Now.DayOfYear < dob.DayOfYear)  {
                age -= 1;  
            }
        
            return new ProcessedApplication() {
                Person = new ApplicationPerson() {
                    PostalCode = rawApplication.ClientData.PostalCode,
                    Flsah = rawApplication.ClientData.FirstLanguage,
                    Age = age
                },
                Roe = new Roe() {
                    FinalPayPeriodDay = rawApplication.Roe.FinalPayPeriodDay,
                    FirstDayForWhichPaid = rawApplication.Roe.FirstDayForWhichPaid,
                    LastDayForWhichPaid = rawApplication.Roe.LastDayForWhichPaid,
                    PayPeriodType = rawApplication.Roe.PayPeriodType,
                    PayPeriods = payPeriods
                },
                ApplicationDate = rawApplication.ClientApplication.ApplicationDate
            };
        }
    }
}