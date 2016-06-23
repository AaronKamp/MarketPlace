CREATE TABLE [dbo].[ServiceType]
(
	[Id] TINYINT NOT NULL IDENTITY(1,1), 
    [Description] VARCHAR(50) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1, 
    [CreatedDate] DATETIME NOT NULL DEFAULT getdate(), 
    [UpdatedDate] DATETIME NOT NULL , 
    CONSTRAINT [PK_ServiceType_Id] PRIMARY KEY ([Id])
)
