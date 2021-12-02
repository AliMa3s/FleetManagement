CREATE TABLE [dbo].[Kleur] (
    [kleurid]   INT  NOT NULL IDENTITY(1,1),
    [kleurnaam] NVARCHAR (150) NOT NULL,
    PRIMARY KEY CLUSTERED ([kleurid] ASC)
);