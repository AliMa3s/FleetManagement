CREATE TABLE [dbo].[Tankkaart_Brandstoftype] (
    [tankkaartnummer] NVARCHAR (25) NOT NULL,
    [brandstoftypeid] INT           NOT NULL,
    CONSTRAINT [PK_Tankkaart_Brandstoftype] PRIMARY KEY CLUSTERED ([tankkaartnummer],[brandstoftypeid] ASC),
    CONSTRAINT [FK_Tankkaart_Brandstoftype_tankkaart] FOREIGN KEY ([tankkaartnummer]) REFERENCES [dbo].[Tankkaart] ([tankkaartnummer]),
    CONSTRAINT [FK_Tankkaart_Brandstoftype_brandstoftype] FOREIGN KEY ([brandstoftypeid]) REFERENCES [dbo].[Brandstoftype] ([brandstoftypeid])
);