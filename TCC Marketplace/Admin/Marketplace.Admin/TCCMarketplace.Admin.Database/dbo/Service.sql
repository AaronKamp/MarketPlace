CREATE TABLE [dbo].[Service] (
    [Id]                        INT             IDENTITY (1, 1) NOT NULL,
    [ServiceTypeId]             TINYINT         NOT NULL,
    [Tilte]                     VARCHAR (255)   NOT NULL,
    [ShortDescription]          VARCHAR (1000)  NOT NULL,
    [LongDescription]           VARCHAR (5000)  NULL,
    [StartDate]                 DATETIME        NOT NULL,
    [EndDate]                   DATETIME        NOT NULL,
    [URL]                       VARCHAR (255)   NOT NULL,
    [PartnerPromoCode]          VARCHAR (255)   NULL,
    [IsActive]                  BIT             NOT NULL,
    [IconImage]                 VARCHAR (255)   NOT NULL,
    [SliderImage]               VARCHAR (255)   NULL,
    [CustomField1]              VARCHAR (255)   NULL,
    [CustomField2]              VARCHAR (255)   NULL,
    [CustomField3]              VARCHAR (255)   NULL,
    [MakeLive]                  BIT             NOT NULL,
    [CreatedDate]               DATETIME        DEFAULT (getdate()) NOT NULL,
    [UpdatedDate]               DATETIME        NOT NULL,
    [UpdatedUser]               VARCHAR (255)   NOT NULL,
    [LastExtractedDate]         DATETIME        NULL,
    [InAppPurchaseId]           VARCHAR (256)   NULL,
    [PurchasePrice]             DECIMAL (10, 2) NOT NULL,
    [ServiceStatusAPIAvailable] BIT             DEFAULT ((0)) NOT NULL,
    [ZipCodes]                  VARCHAR (2000)  NULL,
    [DisableAPIAvailable]       BIT             DEFAULT ((0)) NOT NULL,
    [ServiceProviderId]             VARCHAR (256)   NULL,
    CONSTRAINT [PK_Service_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Service_ServiceType_ServiceTypeId] FOREIGN KEY ([ServiceTypeId]) REFERENCES [dbo].[ServiceType] ([Id])
);




