CREATE TABLE [dbo].[State]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
    [State_Name] VARCHAR(100) NOT NULL, 
    [State_Code] VARCHAR(2) NOT NULL, 
    [CreatedDate] DATETIME NOT NULL DEFAULT getdate(), 
    [UpdatedDate] DATETIME NOT NULL , 
	[CountryId] INT NOT NULL, 
    CONSTRAINT [PK_State_Id] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_State_Country_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Country]([Id])
)