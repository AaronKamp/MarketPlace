
ALTER TABLE ServiceType
ADD CONSTRAINT DF_ServiceTypeCreatedDate DEFAULT getdate() FOR CreatedDate,
	CONSTRAINT DF_ServiceTypeIsActive DEFAULT 1 FOR IsActive; 


ALTER TABLE Service
ADD CONSTRAINT DF_ServiceCreatedDate DEFAULT getdate() FOR CreatedDate,
	CONSTRAINT DF_ServiceStatusAPIAvaliable DEFAULT 0 FOR [ServiceStatusAPIAvailable],
	CONSTRAINT DF_DisableAPIAvailable DEFAULT 0 FOR [DisableAPIAvailable];

ALTER TABLE ServiceProduct 
ADD  CONSTRAINT [UK_ServiceProduct_Product_ServiceId_ProductId] UNIQUE NONCLUSTERED ([ServiceId] ASC, [ProductId] ASC);

ALTER TABLE ServiceSCF
ADD  CONSTRAINT [UK_ServiceSCF_Service_ServiceId_SCFId] UNIQUE NONCLUSTERED ([ServiceId] ASC, [SCFId] ASC);

ALTER TABLE Country
ADD CONSTRAINT DF_CountryCreatedDate DEFAULT getdate() FOR CreatedDate; 

ALTER TABLE State
ADD CONSTRAINT DF_StateCreatedDate DEFAULT getdate() FOR CreatedDate; 


ALTER TABLE SCF
ADD CONSTRAINT DF_SCFCreatedDate DEFAULT getdate() FOR CreatedDate,
	CONSTRAINT DF_SCFIsActive DEFAULT 1 FOR IsActive; 

ALTER TABLE [SCF] DROP COLUMN [DisplayText];
ALTER TABLE [SCF]
   ADD  [DisplayText] AS  (([SCF_Code]+' - ')+[City_Name]) PERSISTED NOT NULL;


ALTER TABLE [ProductCategory]
ADD 
	CONSTRAINT DF_ProductCategoryCreatedDate DEFAULT getdate() FOR CreatedDate,
	CONSTRAINT DF_ProductCategoryIsActive DEFAULT 1 FOR IsActive; 


ALTER TABLE [Product]
ADD CONSTRAINT DF_ProductCreatedDate DEFAULT getdate() FOR CreatedDate,
	CONSTRAINT DF_ProductIsActive DEFAULT 1 FOR IsActive; 

ALTER TABLE [ServiceCategory]
ADD CONSTRAINT DF_ServiceCategoryCreatedDate DEFAULT getdate() FOR CreatedDate,
	CONSTRAINT DF_ServiceCategoryIsActive DEFAULT 1 FOR IsActive; 
; 

ALTER TABLE [Status]
ADD CONSTRAINT DF_StatusCreatedDate DEFAULT getdate() FOR CreatedDate,
	CONSTRAINT DF_StatusIsActive DEFAULT 1 FOR IsActive; 


ALTER TABLE [AspNetUsers]
ADD CONSTRAINT DF_UserCreatedDate DEFAULT getdate() FOR CreatedDate,
	CONSTRAINT DF_UserUpdatedDate DEFAULT getdate() FOR UpdatedDate; 

ALTER TABLE [ExtractFrequency]
ADD CONSTRAINT DF_ExtractFrequencyCreatedDate DEFAULT getdate() FOR CreatedDate,
	CONSTRAINT DF_ExtractFrequencyUpdatedDate DEFAULT getdate() FOR UpdatedDate,
	CONSTRAINT DF_ExtractFrequencyIsActive DEFAULT 0 FOR TotalFailedExtracts; 

ALTER TABLE [Frequency]
ADD CONSTRAINT DF_FrequncyCreatedDate DEFAULT getdate() FOR CreatedDate,
	CONSTRAINT DF_FrequencyUpdatedDate DEFAULT getdate() FOR UpdatedDate,
	CONSTRAINT DF_FrequencyIsActive DEFAULT 1 FOR IsActive; 

DROP TABLE [dbo].[ServicesView];



