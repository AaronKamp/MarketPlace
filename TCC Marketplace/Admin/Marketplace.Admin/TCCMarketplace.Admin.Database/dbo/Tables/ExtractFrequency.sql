CREATE TABLE [dbo].[ExtractFrequency] (
    [Id]                     INT          IDENTITY (1, 1) NOT NULL,
    [FrequencyId]            TINYINT      NOT NULL,
    [LastRunDate]            DATETIME     NULL,
    [NextRunDate]            DATETIME     NULL,
    [IsLastExecutionSuccess] BIT          NULL,
    [TotalFailedExtracts]    INT          DEFAULT ((0)) NULL,
    [CreatedDate]            DATETIME     DEFAULT (getdate()) NOT NULL,
    [UpdatedDate]            DATETIME     DEFAULT (getdate()) NOT NULL,
    [UpdatedUser]            VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ExtractFrequency_Frequency_FrequencyId] FOREIGN KEY ([FrequencyId]) REFERENCES [dbo].[Frequency] ([Id])
);

