CREATE TABLE [dbo].[ServiceSCF] (
    [ServiceId] INT NOT NULL,
    [SCFId]     INT NOT NULL,
    CONSTRAINT [PK_ServiceSCF_Id] PRIMARY KEY CLUSTERED ([SCFId] ASC, [ServiceId] ASC),
    CONSTRAINT [FK_ServiceSCF_SCF_SCFId] FOREIGN KEY ([SCFId]) REFERENCES [dbo].[SCF] ([Id]),
    CONSTRAINT [FK_ServiceSCF_Service_ServiceId] FOREIGN KEY ([ServiceId]) REFERENCES [dbo].[Service] ([Id]),
    CONSTRAINT [UK_ServiceSCF_Service_ServiceId_SCFId] UNIQUE NONCLUSTERED ([ServiceId] ASC, [SCFId] ASC)
);


