CREATE TABLE [dbo].[Adres] (
    [adresid]  INT          IDENTITY (1, 1) NOT NULL,
    [straat]   NVARCHAR(55) NULL,
    [nummer]   NVARCHAR(10)   NULL,
    [gemeente] NVARCHAR(55)   NULL,
    [postcode] NVARCHAR(6)   NULL,
    PRIMARY KEY CLUSTERED ([adresid] ASC)
);