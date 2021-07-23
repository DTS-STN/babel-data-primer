-- STEP 1: Gather ids from the Application and client tables
DROP TABLE temp_babel_persons;

CREATE TABLE temp_babel_persons
AS
SELECT 
    eia.ei_client_individual__id as client_id, 
    eia.application_started_date,
    --cli.yob as year_of_birth,
    cli.person_uid
    --cli.gender__id,
    --cli.language__id_spoken,
    --cli.education_level__id,
    --cli.address__id_mailing
FROM v_neiw_ei_application eia
INNER JOIN v_neiw_ei_client_indiv_mod cli on eia.ei_client_individual__id = cli.id
WHERE eia.benefit_type__id = 3 -- Maternity Benefits
AND eia.application_status__id = 3 -- E-filed
AND cli.person_uid is not null
AND eia.application_started_date > DATE '2019-01-01' -- Only records after Jan 1, 2019
AND cli.address__id_mailing is not null;

--SELECT COUNT(*) FROM temp_babel_persons;


-- STEP 2: Gather ids from the ROE table
DROP TABLE temp_babel_roes;

CREATE TABLE temp_babel_roes
AS
SELECT id AS roe_id,
    employee_uid as person_uid
    --final_pay_period_end_date,
    --first_day_worked_date,
    --last_day_paid_date,
    --total_insurable_earning_amt,
    --total_insurable_hour_nbr,
    --pyprdtp__id as pay_period_type
FROM v_roes_roe 
WHERE date_created > DATE '2019-01-01' -- Only records after Jan 1, 2019
AND employee_uid is not null
AND pyprdtp__id in (23,25,22,28) -- Only "Regular" payment types
AND total_insurable_hour_nbr > 600; -- Minimum requirement for maternity

--SELECT COUNT(*) FROM temp_babel_roes;

-- STEP 3: Remove records that are associated to a person that has more than one ROE
DROP TABLE temp_babel_persons_to_remove;

CREATE TABLE temp_babel_persons_to_remove
AS
select person_uid FROM (
    select person_uid, count(roe_id) as ct
    from temp_babel_roes
    group by person_uid
    ) tt
    WHERE tt.ct > 1;

--SELECT COUNT(*) FROM temp_babel_persons_to_remove;

DELETE FROM temp_babel_roes
WHERE person_uid in (select person_uid from temp_babel_persons_to_remove);

--SELECT COUNT(*) FROM temp_babel_roes;

-- STEP 4: Join the temp tables on the person id
DROP TABLE temp_babel_join;

CREATE TABLE temp_babel_join
AS
SELECT tbp.client_id,
    tbp.person_uid,
    tbp.application_started_date,
    tbr.roe_id
FROM temp_babel_persons tbp
INNER JOIN temp_babel_roes tbr on tbp.person_uid = tbr.person_uid;

SELECT COUNT(*) FROM temp_babel_join;


-- STEP 5: Take a smaller subset of the data
DROP TABLE temp_babel_join_subset;
CREATE TABLE temp_babel_join_subset 
AS
SELECT * FROM temp_babel_join SAMPLE(1);

SELECT COUNT(*) FROM temp_babel_join_subset;


-- STEP 6: Perform the major join to get all the relevant data

DROP TABLE temp_babel_cli_roe;

CREATE TABLE temp_babel_cli_roe
AS
SELECT 
    tbj.roe_id,
    tbj.client_id,
    tbj.application_started_date,
    
    cli.person_uid,
    cli.yob as year_of_birth,
    cli.gender__id as gender_id,
    gen.ENGLISH_NAME as gender,
    cli.education_level__id as education_level_id,
    edu.ENGLISH_NAME as education_level,
    cli.language__id_spoken as language_id,
    lan.ENGLISH_NAME as language_spoken,

    adr.municipality_name as municipality,
    adr.POSTAL_CODE2 as postal_code,
    prov.ENGLISH_NAME as province,
    
    roe.pyprdtp__id as pay_period_type_id,
    ppt.PAY_PERIOD_TYPE_NAME_EN as pay_period_type,
    roe.final_pay_period_end_date,
    roe.first_day_worked_date,
    roe.last_day_paid_date,
    roe.total_insurable_earning_amt,
    roe.total_insurable_hour_nbr
    
FROM temp_babel_join_subset tbj
INNER JOIN v_neiw_ei_client_indiv_mod cli on cli.id = tbj.client_id
INNER JOIN v_roes_roe roe on roe.id = tbj.roe_id
INNER JOIN v_neiw_languages lan on cli.language__id_spoken = lan.id
INNER JOIN v_neiw_gender gen on cli.gender__id = gen.id
INNER JOIN v_neiw_education_level edu on cli.education_level__id = edu.id
INNER JOIN v_neiw_address adr on adr.id = cli.address__id_mailing
INNER JOIN v_neiw_province prov on prov.id = adr.province__id
INNER JOIN v_roes_pay_period_tp ppt on roe.pyprdtp__id = ppt.id
FETCH FIRST 1500 ROWS ONLY;

SELECT * FROM temp_babel_cli_roe;
SELECT COUNT(*) from temp_babel_cli_roe;


select * from SAEBPROXY.temp_babel_cli_roe;

-- STEP 7: Use join table to get ROEs
drop table temp_babel_roe_earnings;

create table temp_babel_roe_earnings
as
SELECT 
    roee.roe__id as roe_id, 
    roee.PAY_PERIOD_NBR, 
    roee.insurable_earning_amt
FROM temp_babel_join_subset tbj
INNER JOIN v_roes_roe_earning roee on tbj.roe_id = roee.roe__id;
--FETCH FIRST 500 ROWS ONLY;

SELECT trcr.roe_id, trcr.pay_period_type, count(*)
FROM temp_babel_cli_roe trcr
inner join temp_babel_roe_earnings trre on trre.roe_id = trcr.roe_id
GROUP BY trcr.roe_id, trcr.pay_period_type

