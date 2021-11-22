CREATE TABLE [dbo].[Voertuig] (
    [voertuigid]      INT           IDENTITY (1, 1) NOT NULL,
    [automodelid]     INT           NOT NULL,
    [brandstoftypeid] INT           NOT NULL,
    [kleurnaam]       NVARCHAR (250) NULL,
    [aantal_deuren]   NVARCHAR (10)  NULL,
    [chassisnummer]   NVARCHAR (25) NOT NULL,
    [nummerplaat]     NVARCHAR (19)  NOT NULL,
    [inboekdatum]     ROWVERSION    NOT NULL,
    PRIMARY KEY CLUSTERED ([voertuigid] ASC),
    UNIQUE NONCLUSTERED ([nummerplaat] ASC),
    UNIQUE NONCLUSTERED ([chassisnummer] ASC),
    CONSTRAINT [FK_Voertuig_automodel] FOREIGN KEY ([automodelid]) REFERENCES [dbo].[Automodel] ([automodelid]),
    CONSTRAINT [FK_Voertuig_Brandstoftype] FOREIGN KEY ([brandstoftypeid]) REFERENCES [dbo].[Brandstoftype] ([brandstofid]),
    
);