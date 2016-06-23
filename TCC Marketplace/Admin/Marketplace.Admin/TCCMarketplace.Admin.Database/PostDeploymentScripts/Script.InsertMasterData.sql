/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

insert into ServiceType
	([Description], IsActive, UpdatedDate)
values
	('Add-ons', 1, getdate()),
	('Offers', 1, getdate());

insert into Country
	(Country_Name, UpdatedDate, Country_Code)
values
	('USA', getdate(), 'US'),
	('Canada', getdate(), 'CA');

insert into [State]
	(State_Name, CountryId, UpdatedDate, State_Code)
values
	('Alaska', 1, getdate(),'AK'),	
	('California', 1, getdate(), 'CA');

insert into [SCF]
	(SCF_Code, City_Name, IsActive, UpdatedDate, StateId)
values
	('995', 'Anchorage', 1, getdate(), 1),
	('996', 'Anchorage', 1, getdate(), 1),
	('997', 'Fairbanks', 1, getdate(), 1),
	('998', 'Juneau', 1, getdate(), 1),
	('900', 'Los Angeles', 1, getdate(), 2),
	('903', 'Inglewood', 1, getdate(), 2),
	('904', 'Santa Monica', 1, getdate(), 2);

insert into ProductCategory
	(Name, IsActive, UpdatedDate)
values
	('Thermostats', 1, getdate()),
	('Security', 1, getdate()),
	('Other', 1, getdate());

insert into Product
	(Name, ProductCategoryId, IsActive, UpdatedDate)
values
	('Trade', 1, 1, getdate()),
	('Retail', 1, 1, getdate()),
	('Redlink 1.0', 1, 1, getdate()),
	('Redlink 2.0', 1, 1, getdate()),
	('Wifi', 1, 1, getdate()),
	('Exclude Thermostats Monitored by Contractors', 1, 1, getdate()),
	('Camera', 2, 1, getdate()),
	('DIY Security', 2, 1, getdate()),
	('Professional Security', 2, 1, getdate()),
	('Water Leak Detection', 2, 1, getdate()),
	('Other', 3, 1, getdate());

insert into ServiceCategory
	(Description, IsActive, UpdatedDate)
values
	('Sample Category 1', 1, getdate()),
	('Sample Category 2', 1, getdate()),
	('Sample Category 3', 1, getdate());

insert into [Status]
(Name, UpdatedDate)
values
('Initialised', getdate()),
('Success', getdate()),
('Failed', getdate());

