CREATE TABLE [dbo].[SCF] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [SCF_Code]    VARCHAR (3)  NOT NULL,
    [City_Name]   VARCHAR (30) NOT NULL,
    [DisplayText] AS           (([SCF_Code]+' - ')+[City_Name]) PERSISTED NOT NULL,
    [IsActive]    BIT          DEFAULT ((1)) NOT NULL,
    [CreatedDate] DATETIME     DEFAULT (getdate()) NOT NULL,
    [UpdatedDate] DATETIME     NOT NULL,
    [StateId]     INT          NOT NULL,
    CONSTRAINT [PK_ZipCode_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ZipCode_State_StateId] FOREIGN KEY ([StateId]) REFERENCES [dbo].[State] ([Id])
);


