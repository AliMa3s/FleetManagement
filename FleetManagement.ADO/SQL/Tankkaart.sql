CREATE TABLE [dbo].[Tankkaart] (
    [tankkaartnummer]      NVARCHAR (25) NOT NULL,
    [bestuurderid]     INT           NULL,
    [geldigheidsdatum] DATE          NOT NULL,
    [pincode]          NVARCHAR (10)  NULL,
    [actief]           TINYINT       NOT NULL,
    [uitgeefdatum]     TIMESTAMP    NOT NULL,
    PRIMARY KEY CLUSTERED ([tankkaartnummer] ASC),
    CONSTRAINT [FK_Tankkaart_Bestuurder] FOREIGN KEY ([bestuurderid]) REFERENCES [dbo].[Bestuurder] ([bestuurderid])
);