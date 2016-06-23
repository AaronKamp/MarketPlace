﻿CREATE TABLE [dbo].[AspNetRoles] (
    [Name] NVARCHAR (256) NOT NULL,
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex]
    ON [dbo].[AspNetRoles]([Name] ASC);

