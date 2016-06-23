CREATE TABLE [dbo].[Country]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
    [Country_Name] VARCHAR(100) NOT NULL, 
    [Country_Code] VARCHAR(2) NOT NULL, 
    [CreatedDate] DATETIME NOT NULL DEFAULT getdate(), 
    [UpdatedDate] DATETIME NOT NULL , 
    CONSTRAINT [PK_Country_Id] PRIMARY KEY ([Id])
)