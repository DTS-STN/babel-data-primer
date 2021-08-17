using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using DataPrimer.Helpers;
using DataPrimer.Rules;
using DataPrimer.Simulation;
using DataPrimer.Fetching;

using esdc_simulation_classes.MaternityBenefits;

namespace DataPrimer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Config
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            var rulesUrl = config["RulesUrl"];
            var simUrl = config["SimulationUrl"];
            var connString = config["DefaultDb"];
            var password = config["SimulationPassword"];
            int maxAmountToFetch = 2000;

            // Dependency Injection
            ILogInfo logger = new ConsoleLogger();
            IProcessData processor = InitProcessor(rulesUrl);
            IStoreData storer = InitStorer(simUrl, password);
            var context = new BabeldbContext(connString);
            IFetchData fetcher = new DbFetcher(context);

            // Execution
            logger.Print("Running the Data primer...");
            var persons = new List<MaternityBenefitsPersonRequest>();

            // Step 1: Fetch the Data
            logger.Print("Fetching raw data");
            var applications = fetcher.FetchApplications(maxAmountToFetch);
            logger.Print($"Number of applications: {applications.Count}");

            // Step 2: Process the data using the rules engine
            logger.Print("Processing data");
            foreach (var application in applications) {
                try {
                    var nextPerson = processor.Process(application);
                    persons.Add(nextPerson);
                } catch (Exception e) {
                    logger.Print($"Error: {e.Message}");
                } 
            }
            
            // Step 3: Store the data
            logger.Print($"Storing data ({persons.Count})");
            storer.Store(persons);

            logger.Print("Data Primer complete");
        }

        /*** DI Helpers ***/
        private static IProcessData InitProcessor(string rulesUrl) {
            var restClient = new RestSharp.RestClient();
            var rulesApi = new RulesApi(restClient, rulesUrl);
            var rulesEngine = new RulesEngine(rulesApi);
            IProcessData processor = new DataProcessor(rulesEngine);
            return processor;
        }

        private static IStoreData InitStorer(string simulationUrl, string password) {
            var restClient = new RestSharp.RestClient();
            var simApi = new SimulationApi(restClient, simulationUrl, password);
            IStoreData storer = new SimulationStore(simApi);
            return storer;
        }
    }
}
