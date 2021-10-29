﻿CREATE TABLE [dbo].[Voertuig] (
    [voertuigid]      INT           NOT NULL,
    [automodelid]     INT           NOT NULL,
    [brandstoftypeid] INT           NOT NULL,
    [kleurid]         INT           NULL,
    [bestuurderid]    INT           NULL,
    [aantal_deuren]   NVARCHAR (1)  NULL,
    [chassisnummer]   NVARCHAR (17) NOT NULL,
    [nummerplaat]     NVARCHAR (9)  NOT NULL,
    [inboekdatum]     ROWVERSION    NOT NULL,
    PRIMARY KEY CLUSTERED ([voertuigid] ASC),
    UNIQUE NONCLUSTERED ([nummerplaat] ASC),
    UNIQUE NONCLUSTERED ([chassisnummer] ASC),
    CONSTRAINT [FK_Voertuig_automodel] FOREIGN KEY ([automodelid]) REFERENCES [dbo].[Automodel] ([automodelid]),
    CONSTRAINT [FK_Voertuig_Brandstoftype] FOREIGN KEY ([brandstoftypeid]) REFERENCES [dbo].[Brandstoftype] ([brandstofid]),
    CONSTRAINT [FK_Voertuig_kleur] FOREIGN KEY ([kleurid]) REFERENCES [dbo].[Kleur] ([kleurid]),
    CONSTRAINT [FK_Voertuig_Bestuurder] FOREIGN KEY ([bestuurderid]) REFERENCES [dbo].[Bestuurder] ([bestuurderid])
);