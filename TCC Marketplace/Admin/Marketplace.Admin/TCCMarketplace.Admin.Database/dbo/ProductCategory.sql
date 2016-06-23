CREATE TABLE [dbo].[ProductCategory]
(
	[Id] TINYINT NOT NULL IDENTITY(1,1), 
    [Name] VARCHAR(100) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1, 
    [CreatedDate] DATETIME NOT NULL DEFAULT getdate(), 
    [UpdatedDate] DATETIME NOT NULL , 
    CONSTRAINT [PK_ProductCategory_Id] PRIMARY KEY ([Id])
)