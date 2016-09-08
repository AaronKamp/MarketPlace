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


INSERT INTO ServiceType
	([Description], IsActive, UpdatedDate)
VALUES
	('Add-ons', 1, getdate()),
	('Offers', 1, getdate());


INSERT INTO ProductCategory
	(Name, IsActive, UpdatedDate)
VALUES
	('Thermostats', 1, getdate()),
	('Security', 1, getdate()),
	('Other', 1, getdate());

	
INSERT INTO ServiceCategory
	([Description], IsActive, UpdatedDate)
VALUES
	('Sample Category 1', 1, getdate()),
	('Sample Category 2', 1, getdate()),
	('Sample Category 3', 1, getdate());


INSERT INTO [Status]
(Name, IsActive, UpdatedDate)
VALUES
	('Initialised', 1 , getdate()),
	('Success', 1 ,getdate()),
	('Failed', 1 ,getdate());

INSERT INTO [Frequency]
	([Name],[CronExpression])
VALUES
	('2 hours','0 0 0/2 * * *'),
	('8 hours','0 0 0/8 * * *'),
	('1 day','0 0 0 */1 * *'),
	('5 days','0 0 0 */5 * *'),
	('7 days','0 0 0 */7 * *');

INSERT INTO [dbo].[AspNetUsers] ([Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName],[CreatedDate],[UpdatedDate] )
	VALUES (N'Administrator@email.em', CAST ('False' AS bit), N'AMEzGt6400O9MyfmZH8z7ZfTlAxnOj12GaSPUWZCObKuIyHlVTiy42rgbTkId+kWKA==', N'414f53ae-576e-4608-b43c-d2ada3cc570a', NULL, CAST ('False' AS bit), CAST ('False' AS bit), NULL, CAST ('False' AS bit), 0, N'Administrator',getdate(),getdate() );


