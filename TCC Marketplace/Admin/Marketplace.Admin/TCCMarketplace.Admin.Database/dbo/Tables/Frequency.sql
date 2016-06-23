CREATE TABLE [dbo].[Frequency] (
    [Id]             TINYINT      IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (75) NOT NULL,
    [IsActive]       BIT          DEFAULT ((1)) NOT NULL,
    [CreatedDate]    DATETIME     DEFAULT (getdate()) NOT NULL,
    [UpdatedDate]    DATETIME     DEFAULT (getdate()) NOT NULL,
    [CronExpression] VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

