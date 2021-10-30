CREATE TABLE [dbo].[Bestuurder] (
    [bestuurderid]        INT            IDENTITY (1, 1) NOT NULL,
    [adresid]             INT            NULL,
    [voornaam]            NVARCHAR (200)  NOT NULL,
    [achternaam]          NVARCHAR (200)  NOT NULL,
    [geboortedatum]       DATE           NOT NULL,
    [rijksregisternummer] NVARCHAR (15)  NOT NULL,
    [rijbewijstype]       NVARCHAR (100) NOT NULL,
    [rijbewijsnummer]     NVARCHAR (20)  NOT NULL,
    [aanmaakDatum]        TIMESTAMP     NOT NULL,
    PRIMARY KEY CLUSTERED ([bestuurderid] ASC),
    UNIQUE NONCLUSTERED ([rijksregisternummer] ASC),
    CONSTRAINT [FK_Bestuurder_Adres] FOREIGN KEY ([adresid]) REFERENCES [dbo].[Adres] ([adresid])
);