CREATE TABLE [dbo].[Automodel] (
    [automodelid]    INT            IDENTITY (1, 1) NOT NULL,
    [merknaam]      NVARCHAR (200)  NOT NULL,
    [automodelnaam] NVARCHAR (200) NOT NULL,
    [autotype]       NVARCHAR (200)  NOT NULL,
    PRIMARY KEY CLUSTERED ([automodelid] ASC)
);