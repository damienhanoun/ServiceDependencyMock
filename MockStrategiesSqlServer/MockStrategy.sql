CREATE TABLE [dbo].[MockStrategy]
(
    [id] NVARCHAR(50) NOT NULL,
	[methodId] NVARCHAR(50) NOT NULL, 
    [serializedStrategy] VARBINARY(MAX) NOT NULL, 
    [creationDate] DATETIME NOT NULL DEFAULT getdate(), 
    CONSTRAINT [PK_MockStrategy] PRIMARY KEY ([id])
)

GO

CREATE INDEX [IX_MockStrategy_Column] ON [dbo].[MockStrategy] ([methodId])
