CREATE TABLE [dbo].[ConfigurationSettings] (
    [Id]                     INT            IDENTITY (1, 1) NOT NULL,
    [CreatedDate]            DATETIME       CONSTRAINT [DF_SettingsCreatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedDate]            DATETIME       CONSTRAINT [DF_SettingsUpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedUser]            NVARCHAR (MAX) NOT NULL,
    [FtpHostAddress]         NVARCHAR (MAX) NOT NULL,
    [FtpPort]                INT            NOT NULL,
    [FtpUser]                NVARCHAR (MAX) NOT NULL,
    [SshPrivateKey]          NVARCHAR (MAX) NOT NULL,
    [IsSshPasswordProtected] BIT            NOT NULL,
    [SshPrivateKeyPassword]  NVARCHAR (MAX) NULL,
    [FtpRemotePath]          NVARCHAR (MAX) NOT NULL,
    [FromEmail]              NVARCHAR (MAX) NOT NULL,
    [FromEmailPassword]      NVARCHAR (MAX) NOT NULL,
    [ToEmails]               NVARCHAR (MAX) NOT NULL,
    [SmtpClient]             NVARCHAR (MAX) NOT NULL,
    [SmtpPort]               INT            NOT NULL,
    CONSTRAINT [PK_dbo.ConfigurationSettings] PRIMARY KEY CLUSTERED ([Id] ASC)
);

