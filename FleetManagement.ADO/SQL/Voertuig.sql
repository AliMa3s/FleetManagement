CREATE TABLE [dbo].[Voertuig] (
    [voertuigid]      INT           NOT NULL,
    [automodelid]     INT           NOT NULL,
    [brandstoftypeid] INT           NOT NULL,
    [kleurnaam]       NVARCHAR(55)  NULL,
    [bestuurderid]    INT           NULL,
    [aantal_deuren]   NVARCHAR (15)  NULL,
    [chassisnummer]   NVARCHAR (20) NOT NULL,
    [nummerplaat]     NVARCHAR (15)  NOT NULL,
    [hybride]         tinyint  (2)  NOT NULL,
    [inboekdatum]     timestamp    NOT NULL,
    
    PRIMARY KEY CLUSTERED ([voertuigid] ASC),
    UNIQUE NONCLUSTERED ([nummerplaat] ASC),
    UNIQUE NONCLUSTERED ([chassisnummer] ASC),
    CONSTRAINT [FK_Voertuig_automodel] FOREIGN KEY ([automodelid]) REFERENCES [dbo].[Automodel] ([automodelid]),
    CONSTRAINT [FK_Voertuig_Brandstoftype] FOREIGN KEY ([brandstoftypeid]) REFERENCES [dbo].[Brandstoftype] ([brandstofid]),
    CONSTRAINT [FK_Voertuig_Bestuurder] FOREIGN KEY ([bestuurderid]) REFERENCES [dbo].[Bestuurder] ([bestuurderid])
);