# Maternity Benefits Policy Difference Engine Data Primer

This is a console application that is responsible for populating the database that is used by the Maternity Benefits policy difference engine (PDE). It can generate mock data or it can process real, raw data into something usable.

## Flow

The full flow involves first fetching the data, then processing it into something usable by the simulation engine, and then storing it.


### Step 1: Data fetching

There are two options for fetching the data. We can either use mock data, or we can fetch real data.

#### Mock data

Fetching the mock data involves generating fake individuals and corresponding Records of Employment. This generation is largely random, but can be informed by real statistics. This process is called imputation. 

If generating mock data, then it is advisable to use explicitly fake identifiers, in order to avoid misleading anyone who sees the PDE in action. For example, if you need to generate a city name, don't use "Ottawa" - use "Fakeville" instead.

#### Real data

Todo...

### Step 2: Processing

This step involves taking the fetched data and computing new properties that are relevant for the Policy Difference Engine. 

The raw data contains data such as RoE earnings and postal codes. In order for this to be usable by the PDE, we need to calculate the average income and the number of best weeks to which an applicant is entitled. The actual code for this is in the Rules engine, so this processing step simply involves making the calls to the rules engine and adding on the properties. The rules engine URL is a config variable.

The result of this step is a list of objects representing "Persons", which can be used as data points in the PDE.

### Step 3: Storing the data

Once the "Persons" are all generated from the raw data, we simply need to store it in the appropriate database. The connection string for the database is a config setting. Each stored person will be used as a data point in the PDE.
