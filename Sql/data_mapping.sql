-- STEP 1: Gather ids from the Application and client tables
DROP TABLE temp_babel_persons;

CREATE TABLE temp_babel_persons
AS
SELECT 
    eia.ei_client_individual__id AS client_id, 
    eia.application_started_date,
    cli.person_uid
FROM v_neiw_ei_application eia
INNER JOIN v_neiw_ei_client_indiv_mod cli ON eia.ei_client_individual__id = cli.id
WHERE eia.benefit_type__id = 3 -- Maternity Benefits
AND eia.application_status__id = 3 -- E-filed
AND cli.person_uid IS NOT NULL
AND eia.application_started_date > DATE '2019-01-01' -- Only records after Jan 1, 2019
AND cli.address__id_mailing IS NOT NULL;

--SELECT COUNT(*) FROM temp_babel_persons;


-- STEP 2: Gather ids from the ROE table
DROP TABLE temp_babel_roes;

CREATE TABLE temp_babel_roes
AS
SELECT 
    id AS roe_id,
    employee_uid AS person_uid
FROM v_roes_roe 
WHERE date_created > DATE '2019-01-01' -- Only records after Jan 1, 2019
AND employee_uid IS NOT NULL
AND pyprdtp__id IN (23,25,22,28) -- Only "Regular" payment types
AND total_insurable_hour_nbr > 600; -- Minimum requirement for maternity

--SELECT COUNT(*) FROM temp_babel_roes;


-- STEP 3: Remove records that are associated to a person that has more than one ROE
DROP TABLE temp_babel_persons_to_remove;

CREATE TABLE temp_babel_persons_to_remove
AS
SELECT person_uid FROM (
    SELECT person_uid, COUNT(roe_id) AS ct
    FROM temp_babel_roes
    GROUP BY person_uid
    ) tt
    WHERE tt.ct > 1;

DELETE FROM temp_babel_roes
WHERE person_uid IN (SELECT person_uid FROM temp_babel_persons_to_remove);

--SELECT COUNT(*) FROM temp_babel_roes;


-- STEP 4: Join the temp tables ON the person id
DROP TABLE temp_babel_join;

CREATE TABLE temp_babel_join
AS
SELECT tbp.client_id,
    tbp.person_uid,
    tbp.application_started_date,
    tbr.roe_id
FROM temp_babel_persons tbp
INNER JOIN temp_babel_roes tbr ON tbp.person_uid = tbr.person_uid;

SELECT COUNT(*) FROM temp_babel_join;


-- STEP 5: Take a smaller subset of the data
DROP TABLE temp_babel_join_subset;
CREATE TABLE temp_babel_join_subset 
AS
SELECT * FROM temp_babel_join SAMPLE(2);

SELECT COUNT(*) FROM temp_babel_join_subset;


-- STEP 6: Perform the major join to get all the relevant data
DROP TABLE temp_babel_cli_roe;

CREATE TABLE temp_babel_cli_roe
AS
SELECT 
    tbj.roe_id,
    tbj.client_id,
    tbj.application_started_date,
    
    -- Client information
    cli.person_uid,
    cli.yob AS year_of_birth,
    cli.gender__id AS gender_id,
    gen.ENGLISH_NAME AS gender,
    cli.education_level__id AS education_level_id,
    edu.ENGLISH_NAME AS education_level,
    cli.language__id_spoken AS language_id,
    lan.ENGLISH_NAME AS language_spoken,
    
    -- Postal code
    adr.municipality_name AS municipality,
    adr.POSTAL_CODE2 AS postal_code,
    prov.ENGLISH_NAME AS province,
    
    -- RoE info
    roe.pyprdtp__id AS pay_period_type_id,
    ppt.PAY_PERIOD_TYPE_NAME_EN AS pay_period_type,
    roe.final_pay_period_end_date,
    roe.first_day_worked_date,
    roe.last_day_paid_date,
    roe.total_insurable_earning_amt,
    roe.total_insurable_hour_nbr
    
FROM temp_babel_join_subset tbj
INNER JOIN v_neiw_ei_client_indiv_mod cli ON cli.id = tbj.client_id
INNER JOIN v_roes_roe roe ON roe.id = tbj.roe_id
INNER JOIN v_neiw_languages lan ON cli.language__id_spoken = lan.id
INNER JOIN v_neiw_gender gen ON cli.gender__id = gen.id
INNER JOIN v_neiw_education_level edu ON cli.education_level__id = edu.id
INNER JOIN v_neiw_address adr ON adr.id = cli.address__id_mailing
INNER JOIN v_neiw_province prov ON prov.id = adr.province__id
INNER JOIN v_roes_pay_period_tp ppt ON roe.pyprdtp__id = ppt.id;

SELECT COUNT(*) from temp_babel_cli_roe;


-- STEP 7: Use join table to get ROEs
DROP TABLE temp_babel_roe_earnings;

CREATE TABLE temp_babel_roe_earnings
AS
SELECT 
    roee.roe__id AS roe_id, 
    roee.PAY_PERIOD_NBR, 
    roee.insurable_earning_amt
FROM temp_babel_join_subset tbj
INNER JOIN v_roes_roe_earning roee ON tbj.roe_id = roee.roe__id;

SELECT COUNT(*) from temp_babel_roe_earnings;

-- STEP 8: Export the two tables (temp_babel_cli_roe and temp_babel_roe_earnings) to csv and import into your own DB
/*
EXPORT:
- Tools> Database Export
- Choose your connection
- Uncheck Export DDL
- Check Export Data
- Format: csv, use header, keep defaults
- Save AS single file
- Types to export: Tables
- Specify Data: Search for your table (temp_babel_cli_roe) and select it

IMPORT (into SSMS):
- Connect to server
- Right-click on server in the Object explorer, find 'Tasks', and select 'Import Flat File'
- Choose the csv file. Use the defaults (ensure table names are 'cli_roe' and 'earnings'
- Import the data
*/