CREATE TABLE [dbo].[Brandstoftype] (
    [brandstofid]   INT           IDENTITY (1, 1) NOT NULL,
    [brandstofnaam] NVARCHAR (200) NOT NULL,
    PRIMARY KEY CLUSTERED ([brandstofid] ASC)
);