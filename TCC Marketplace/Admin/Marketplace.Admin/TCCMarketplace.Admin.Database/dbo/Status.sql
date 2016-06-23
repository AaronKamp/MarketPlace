CREATE TABLE [dbo].[Status]
(
	[Id] TINYINT NOT NULL IDENTITY(1,1), 
    [Name] VARCHAR(50) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1, 
    [CreatedDate] DATETIME NOT NULL DEFAULT getdate(), 
    [UpdatedDate] DATETIME NOT NULL , 
    CONSTRAINT [PK_Status_Id] PRIMARY KEY ([Id])
)
