CREATE TABLE [dbo].[Automodel] (
    [automodelid]    INT            IDENTITY (1, 1) NOT NULL,
    [merk_naam]      NVARCHAR (200)  NOT NULL,
    [automodel_naam] NVARCHAR (200) NOT NULL,
    [autotype]       NVARCHAR (200)  NOT NULL,
    PRIMARY KEY CLUSTERED ([automodelid] ASC)
);