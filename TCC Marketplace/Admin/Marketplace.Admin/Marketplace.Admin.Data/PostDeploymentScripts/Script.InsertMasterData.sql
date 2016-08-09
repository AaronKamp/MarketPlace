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

SET IDENTITY_INSERT [dbo].[ConfigurationSettings] ON;
INSERT INTO [dbo].[ConfigurationSettings] ([Id], [UpdatedUser], [FtpHostAddress], [FtpPort], [FtpUser], [SshPrivateKey], [IsSshPasswordProtected], [SshPrivateKeyPassword], [FtpRemotePath], [FromEmail], [FromEmailPassword], [ToEmails], [SmtpClient], [SmtpPort])
	VALUES (1, N'Administrator', N'14.141.33.202', 2122, N'tccmarketplace', N'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', CAST ('True' AS bit), N'l6HsfppurYTMpw4OflitwHA74t6bi7fxRLZGw8TknzQ=', N'/mnt/data_500G/tccmarketplace/', N'azure_4746a01d067c4bdb2cb6b325000e07cc@azure.com', N'SzCPzKvtRt5JtPAXZLuERkZ2O6QbFuw+3bnhP/nqtFw=', N'ajith@performixbiz.com, ajithkhan@gmail.com', N'smtp.sendgrid.net', 587);
SET IDENTITY_INSERT [dbo].[ConfigurationSettings] OFF;


