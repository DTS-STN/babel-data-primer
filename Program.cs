using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using DataPrimer.Helpers;
using DataPrimer.Rules;
using DataPrimer.Simulation;
using DataPrimer.Storage;

using esdc_simulation_classes.MaternityBenefits;

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
            var rulesUrl = config["RulesUrl"];
            var simUrl = config["SimulationUrl"];
            var connString = config["DefaultDb"];

            // DI
            ILogInfo logger = new ConsoleLogger();
            
            IProcessData processor = InitProcessor(rulesUrl);
            IStoreData storer = InitStorer(simUrl);
            
            //IFetchData fetcher = new MockFetcher(2);
            var context = new BabeldbContext(connString);
            IFetchData fetcher = new DbFetcher(context, 100);

            // Execution
            logger.Print("Running the Data primer...");
            var persons = new List<MaternityBenefitsPersonRequest>();

            logger.Print("Fetching raw data");
            var applications = fetcher.FetchApplications();
            logger.Print($"Number of applications: {applications.Count}");

            logger.Print("Processing data");
            foreach (var application in applications) {
                try {
                    var nextPerson = processor.Process(application);
                    //logger.Print($"Processed...");
                    persons.Add(nextPerson);
                } catch (Exception e) {
                    logger.Print($"Error: {e.Message}");
                } 
            }
            
            logger.Print($"Storing data ({persons.Count})");
            storer.Store(persons);

            logger.Print("Data Primer complete");
        }

        private static IProcessData InitProcessor(string rulesUrl) {
            var restClient = new RestSharp.RestClient();
            var rulesApi = new RulesApi(restClient, rulesUrl);
            var rulesEngine = new RulesEngine(rulesApi);
            IProcessData processor = new DataProcessor(rulesEngine);
            return processor;
        }

        private static IStoreData InitStorer(string simulationUrl) {
            var restClient = new RestSharp.RestClient();
            var simApi = new SimulationApi(restClient, simulationUrl);
            IStoreData storer = new SimulationStore(simApi);
            return storer;
        }
    }
}
