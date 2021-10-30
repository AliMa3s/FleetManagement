CREATE TABLE [dbo].[Tankkaart] (
    [kaartnummer]      NVARCHAR (25) NOT NULL,
    [bestuurderid]     INT           NULL,
    [geldigheidsdatum] DATE          NOT NULL,
    [pincode]          NVARCHAR (10)  NULL,
    [actief]           TINYINT       NOT NULL,
    [uitgeefdatum]     TIMESTAMP    NOT NULL,
    PRIMARY KEY CLUSTERED ([kaartnummer] ASC),
    CONSTRAINT [FK_Tankkaart_Bestuurder] FOREIGN KEY ([bestuurderid]) REFERENCES [dbo].[Bestuurder] ([bestuurderid])
);