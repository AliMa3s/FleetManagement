CREATE TABLE [dbo].[Brandstoftype] (
    [brandstoftypeid]   INT           IDENTITY (1, 1) NOT NULL,
    [brandstofnaam] NVARCHAR (150) NOT NULL,
    PRIMARY KEY CLUSTERED ([brandstoftypeid] ASC)
);