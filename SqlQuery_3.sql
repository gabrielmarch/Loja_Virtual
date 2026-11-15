
USE LojaVirtualDb;
GO

ALTER TABLE dbo.Pagamento
ADD
    PedidoId INT NOT NULL,
    MetodoPagamento NVARCHAR(30) NOT NULL,
    Valor DECIMAL(18,2) NOT NULL,
    DataPagamento DATETIME2 NOT NULL,
    Status NVARCHAR(30) NOT NULL;
GO

-- Relacionamento com Pedidos
ALTER TABLE dbo.Pagamento
ADD CONSTRAINT FK_Pagamento_Pedidos
FOREIGN KEY (PedidoId)
REFERENCES dbo.Pedidos(Id);
GO

-- Um pagamento por pedido
CREATE UNIQUE INDEX IX_Pagamento_PedidoId
ON dbo.Pagamento(PedidoId);
GO
