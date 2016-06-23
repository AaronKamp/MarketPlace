CREATE TABLE [dbo].[Product]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
	[Name] varchar(100) not null,
	[ProductCategoryId] tinyint not null,
	[IsActive] BIT NOT NULL DEFAULT 1, 
    [CreatedDate] DATETIME NOT NULL DEFAULT getdate(), 
    [UpdatedDate] DATETIME NOT NULL,
	CONSTRAINT [PK_Product_Id] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_Product_ProductCategory_ProductCategoryId] FOREIGN KEY ([ProductCategoryId]) REFERENCES [ProductCategory]([Id])
)
