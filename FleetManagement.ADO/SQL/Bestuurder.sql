CREATE TABLE [dbo].[Bestuurder] (
    [bestuurderid]        INT            IDENTITY (1, 1) NOT NULL,
    [adresid]             INT            NULL,
    [voornaam]            NVARCHAR (50)  NOT NULL,
    [achternaam]          NVARCHAR (50)  NOT NULL,
    [geboortedatum]       DATE           NOT NULL,
    [rijksregisternummer] NVARCHAR (11)  NOT NULL,
    [rijbewijstype]       NVARCHAR (100) NOT NULL,
    [rijbewijsnummer]     NVARCHAR (10)  NOT NULL,
    [aanmaakDatum]        ROWVERSION     NOT NULL,
    PRIMARY KEY CLUSTERED ([bestuurderid] ASC),
    UNIQUE NONCLUSTERED ([rijksregisternummer] ASC),
    CONSTRAINT [FK_Table_Adres] FOREIGN KEY ([adresid]) REFERENCES [dbo].[Adres] ([adresid])
);