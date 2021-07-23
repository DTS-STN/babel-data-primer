# Maternity Benefits Policy Difference Engine Data Primer

This is a console application that is responsible for populating the database that is used by the Maternity Benefits policy difference engine (PDE). It can generate mock data or it can process real, raw data into something usable.

## Flow

### Step 0a: Filtering and Joining Data
The raw data is currently stored in the data lake in the NEIW and RoE databases. Prior to the execution of this data primer, a series of queries must be run against this data lake to filter the data down into data that makes sense to use on the PDE. This includes steps like filtering out older applications and non-maternity EI applications. The set of queries is stored in the Sql/data_mapping.sql file in this repo. The result of running these queries is a set of two new custom temporary tables.

One of these tables contains application/client/RoE data. The other contains the pay period earnings for the associated RoE. There is a foreign key relationship on the RoE ID between these two tables, which will be used by the data primer to associate the RoE with the earnings.

### Step 0b: Exporting/Importing the Custom Tables
Once the custom tables have been generated from the SQL queries, the data must be exported from the data lake, and then imported into our own database (to which the Data Primer will connect). This can be done using the export and import tools in the various SQL management clients (Oracle SQL Developer, SSMS, etc). The tables can be exported as CSV and then imported into our own database.


### Step 1: Data fetching
Once the raw data has been filtered and stored in our own database, we can run the data primer. The first step of the data primer is to fetch this data from the custom tables that have been imported into our database. The Data Primer uses Entity Framework to interact with the database. If the data that is filtered into the custom tables is ever changed, then a migration may need to be run, so that the Data Primer can interact properly with the database. On hooking up the program to Entity Framework, it is able to generate the Earnings and CliRoe classes, which represent the data from the custom databases. The DbFetcher class is responsible for fetching from the database and converting the data into a set of C# objects that are ready for further processing. 

### Step 2: Processing
The processing step involves taking the C# form of the db data (type 'ProcessedApplication' from step 1), and transforming it into an object that fulfills the data contract of the PDE (type 'MaternityBenefitsPersonRequest', defined in the Simulation API Nuget package). It must calculate the various properties that are specified in the contract. This involves making requests to the Rules API, where the actual calculation logic is stored. There is very little actual business logic in the Data Primer itself - instead it makes API requests to other systems that are built to handle the logic. 

Most of the required data for the data contract is already found in the db data, such as language and province. These are trivial mappings that the Data Primer takes care of during the processing step. The complex one is the Average Weekly Income (AWI) for a given person. This calculation involves taking into account RoE information, EI application, personal information, and the RoE earnings. This is the most complicated step in the entire PDE flow. Again, the work here is handled by the Rules Engine - the Data Primer simply forms the required request objects from the data and calls the Rules Engine using an API request.

Another factor of the AWI calculation is the number of best weeks. This number is based on the unemployment rate in the applicant's economic region. This value is subject to regular changes. It is possible to obtain this value from the applicant's postal code. This retrieval is a separate call in the Rules Engine, so for each applicant, the Data Primer must make TWO calls to the Rules Engine:
- The first is a request to the /BestWeeks endpoint, and the request object contains in the applicant's postal code. The result is the number of best weeks.
- The second is a request to the /AverageIncome endpoint. The request object contains the number of best weeks from the previous call, as well as a bunch of other RoE/application/RoE pay period information. This is where we find the complex calculation. The result of this call is the AWI for the applicant.

Once the Data Primer has made the two calls and obtained the AWI, it packages it with the other demographic data into the MaternityBenefitsPersonRequest object, which is the data contract for the PDE. 

### Step 3: Storing the Data
Now that the data has been processed to meet the requirements for the PDE, we need to store it in the PDE DB. This is a relatively straightforward step that involves passing the data directly to the Simulation Engine (via Web API request), so that the simulation engine can store it.


## Usage
The Data Primer does not need to be run very often. It only needs to be run when the data required by the PDE needs to be updated. This may include (but are not limited to) the following reasons:
- The raw data lake has been updated with new data, so we want to have newer (and therefore more relevant) data being used by the PDE
- The Rules Engine has been updated to handle more complex scenarios, and we can bring in more of the existing data that was previously being filtered out. For example, the rules engine is currently unable to handle EI applications that have multiple RoE's, so these are filtered out (in the SQL Queries). If the Rules Engine were updated to handle these cases, then the query could be updated to remove that filter, and the data could be re-seeded.
- We want to import more demographic data for further aggregations and visualizations. In this case, the SQL Queries would be updated to pull in the desired data, and the Data Primer and Simulation Engine would need to be updated accordingly.

Since the Data Primer is a console application that should only need to be run rarely, it is not deployed or automated anywhere. It can simply be pulled down and executed using `dotnet run`. Before running, ensure that the environment variables (in appsettings.json) are set appropriately. These include:
- RulesUrl: This is the Url for the Rules Engine API
- SimulationUrl: Url for the Simulation Engine API
- DefaultDB: Connection string for the Database that stores the custom tables that are queried during the fetching step


