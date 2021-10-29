CREATE TABLE [dbo].[Adres] (
    [adresid]  INT          IDENTITY (1, 1) NOT NULL,
    [straat]   VARCHAR (55) NULL,
    [nummer]   NCHAR (10)   NULL,
    [gemeente] NCHAR (55)   NULL,
    [postcode] NCHAR (55)   NULL,
    PRIMARY KEY CLUSTERED ([adresid] ASC)
);