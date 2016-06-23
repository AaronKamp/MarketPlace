CREATE TABLE [dbo].[ServiceCategory]
(
	[Id] TINYINT NOT NULL IDENTITY(1,1), 
    [Description] VARCHAR(50) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1, 
    [CreatedDate] DATETIME NOT NULL DEFAULT getdate(), 
    [UpdatedDate] DATETIME NOT NULL , 
    CONSTRAINT [PK_ServiceCategory_Id] PRIMARY KEY ([Id])
)
