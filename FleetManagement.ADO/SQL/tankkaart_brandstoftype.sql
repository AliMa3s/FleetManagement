CREATE TABLE [dbo].[Tankkaart_Brandstoftype] (
    [tankkaartnummer] NVARCHAR (19) NOT NULL,
    [brandstoftypeid] INT           NOT NULL,
    CONSTRAINT [PK_Tankkaart_Brandstoftype] PRIMARY KEY CLUSTERED ([tankkaartnummer] ASC),
    CONSTRAINT [FK_Tankkaart_Brandstoftype_tankkaart] FOREIGN KEY ([tankkaartnummer]) REFERENCES [dbo].[Tankkaart] ([kaartnummer]),
    CONSTRAINT [FK_Tankkaart_Brandstoftype_brandstoftype] FOREIGN KEY ([brandstoftypeid]) REFERENCES [dbo].[Brandstoftype] ([brandstofid])
);