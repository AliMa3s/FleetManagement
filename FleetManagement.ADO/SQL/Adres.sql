CREATE TABLE [dbo].[Adres] (
    [adresid]  INT          IDENTITY (1, 1) NOT NULL,
    [straat]   NVARCHAR(200)   NULL,
    [nummer]   NVARCHAR(50)   NULL,
    [gemeente] NVARCHAR(200)   NULL,
    [postcode] NVARCHAR(15)    NULL,
    PRIMARY KEY CLUSTERED ([adresid] ASC)
);