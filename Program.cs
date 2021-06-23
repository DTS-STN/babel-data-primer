using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using DataPrimer.Models;
using DataPrimer.Helpers;
using DataPrimer.Rules;
using DataPrimer.Storage;

namespace DataPrimer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read config
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            var rulesUrl = config.GetValue<string>("RulesUrl");
            var connectionString = config.GetConnectionString("DefaultDB");

            // DI
            ILogInfo logger = new ConsoleLogger();
            IFetchData fetcher = new MockFetcher(2);
            IProcessData processor = InitProcessor(rulesUrl);
            IStoreData storer = InitStorer(connectionString);

            // Execution
            logger.Print("Running the Data primer...");
            var persons = new List<Persons>();

            logger.Print("Fetching raw data");
            var applications = fetcher.FetchApplications();
            logger.Print($"Number of applications: {applications.Count}");

            logger.Print("Processing data");
            foreach (var application in applications) {
                var nextPerson = processor.Process(application);
                logger.Print($"Processed: {nextPerson.Id}");
                persons.Add(nextPerson);
            }
            
            logger.Print("Storing data");
            storer.Store(persons);

            logger.Print("Data Primer complete");
        }

        private static IProcessData InitProcessor(string rulesUrl) {
            var restClient = new RestSharp.RestClient();
            var rulesApi = new RulesApi(restClient, rulesUrl);
            var rulesEngine = new RulesEngine(rulesApi);
            var averageIncomeGetter = new AverageIncomeGetter(rulesEngine);
            var bestWeeksGetter = new BestWeeksGetter(rulesEngine);
            IProcessData processor = new DataProcessor(bestWeeksGetter, averageIncomeGetter);
            return processor;
        }

        private static IStoreData InitStorer(string connectionString) {
            var dbContext = new AppDbContext(connectionString);
            IStoreData storer = new SimulationEFStore(dbContext);
            return storer;
        }
    }
}
