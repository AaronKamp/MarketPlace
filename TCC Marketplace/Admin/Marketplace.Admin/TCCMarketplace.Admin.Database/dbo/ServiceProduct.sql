CREATE TABLE [dbo].[ServiceProduct] (
    [ServiceId] INT NOT NULL,
    [ProductId] INT NOT NULL,
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_ServiceProduct] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ServiceProduct_Product_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id]),
    CONSTRAINT [FK_ServiceProduct_Service_ServiceId] FOREIGN KEY ([ServiceId]) REFERENCES [dbo].[Service] ([Id]),
    CONSTRAINT [UK_ServiceProduct_Product_ServiceId_ProductId] UNIQUE NONCLUSTERED ([ServiceId] ASC, [ProductId] ASC)
);


