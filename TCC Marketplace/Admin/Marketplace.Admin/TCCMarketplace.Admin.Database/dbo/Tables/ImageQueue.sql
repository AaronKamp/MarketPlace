CREATE TABLE [dbo].[ImageQueue] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [ImageUrl]          NVARCHAR (MAX) NOT NULL,
    [ActualDeletedDate] DATETIME       NOT NULL,
    [IsDeleted]         BIT            NOT NULL,
    [PurgedDate]        DATETIME       NULL,
    [DeletedUser]       NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_dbo.ImageQueue] PRIMARY KEY CLUSTERED ([Id] ASC)
);