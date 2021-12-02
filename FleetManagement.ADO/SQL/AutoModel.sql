CREATE TABLE [dbo].[Automodel] (
    [automodelid]    INT            IDENTITY (1, 1) NOT NULL,
    [merknaam]      NVARCHAR (150)  NOT NULL,
    [automodelnaam] NVARCHAR (150) NOT NULL,
    [autotype]       NVARCHAR (150)  NOT NULL,
    PRIMARY KEY CLUSTERED ([automodelid] ASC)
);