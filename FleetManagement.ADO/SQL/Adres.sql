CREATE TABLE [dbo].[Adres] (
    [adresid]  INT          IDENTITY (1, 1) NOT NULL,
    [straat]   NVARCHAR(150)   NULL,
    [nummer]   NVARCHAR(150)   NULL,
    [gemeente] NVARCHAR(150)   NULL,
    [postcode] NVARCHAR(150)    NULL,
    PRIMARY KEY CLUSTERED ([adresid] ASC),
);