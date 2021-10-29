CREATE TABLE [dbo].[Automodel] (
    [automodelid]    INT            NOT NULL,
    [merk_naam]      NVARCHAR (55)  NOT NULL,
    [automodel_naam] NVARCHAR (100) NOT NULL,
    [autotype]       NVARCHAR (55)  NOT NULL,
    PRIMARY KEY CLUSTERED ([automodelid] ASC)
);