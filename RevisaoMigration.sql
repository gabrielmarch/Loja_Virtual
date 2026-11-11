BEGIN TRANSACTION;
DECLARE @var nvarchar(max);
SELECT @var = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Clientes]') AND [c].[name] = N'Telefone');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Clientes] DROP CONSTRAINT ' + @var + ';');
ALTER TABLE [Clientes] ALTER COLUMN [Telefone] nvarchar(20) NOT NULL;

DECLARE @var1 nvarchar(max);
SELECT @var1 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Clientes]') AND [c].[name] = N'SenhaHash');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Clientes] DROP CONSTRAINT ' + @var1 + ';');
ALTER TABLE [Clientes] ALTER COLUMN [SenhaHash] nvarchar(200) NOT NULL;

DECLARE @var2 nvarchar(max);
SELECT @var2 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Clientes]') AND [c].[name] = N'RazaoSocial');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Clientes] DROP CONSTRAINT ' + @var2 + ';');
ALTER TABLE [Clientes] ALTER COLUMN [RazaoSocial] nvarchar(200) NULL;

DECLARE @var3 nvarchar(max);
SELECT @var3 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Clientes]') AND [c].[name] = N'NomeFantasia');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Clientes] DROP CONSTRAINT ' + @var3 + ';');
ALTER TABLE [Clientes] ALTER COLUMN [NomeFantasia] nvarchar(200) NULL;

DECLARE @var4 nvarchar(max);
SELECT @var4 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Clientes]') AND [c].[name] = N'Nome');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Clientes] DROP CONSTRAINT ' + @var4 + ';');
ALTER TABLE [Clientes] ALTER COLUMN [Nome] nvarchar(150) NOT NULL;

DECLARE @var5 nvarchar(max);
SELECT @var5 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Clientes]') AND [c].[name] = N'Endereco');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Clientes] DROP CONSTRAINT ' + @var5 + ';');
ALTER TABLE [Clientes] ALTER COLUMN [Endereco] nvarchar(200) NOT NULL;

DECLARE @var6 nvarchar(max);
SELECT @var6 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Clientes]') AND [c].[name] = N'Email');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Clientes] DROP CONSTRAINT ' + @var6 + ';');
ALTER TABLE [Clientes] ALTER COLUMN [Email] nvarchar(100) NOT NULL;

DECLARE @var7 nvarchar(max);
SELECT @var7 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Clientes]') AND [c].[name] = N'CPF');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Clientes] DROP CONSTRAINT ' + @var7 + ';');
ALTER TABLE [Clientes] ALTER COLUMN [CPF] nvarchar(14) NULL;

DECLARE @var8 nvarchar(max);
SELECT @var8 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Clientes]') AND [c].[name] = N'CNPJ');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Clientes] DROP CONSTRAINT ' + @var8 + ';');
ALTER TABLE [Clientes] ALTER COLUMN [CNPJ] nvarchar(18) NULL;

CREATE TABLE [Cupons] (
    [idCupom] int NOT NULL IDENTITY,
    [CodigoCupom] nvarchar(50) NOT NULL,
    [Desconto] decimal(18,2) NOT NULL,
    [DataValidade] datetime2 NOT NULL,
    [ativo] bit NOT NULL,
    CONSTRAINT [PK_Cupons] PRIMARY KEY ([idCupom])
);

CREATE TABLE [Fornecedores] (
    [idFornecedor] int NOT NULL IDENTITY,
    [NomeFornecedor] nvarchar(200) NOT NULL,
    [EmailFornecedor] nvarchar(100) NOT NULL,
    [TelefoneFornecedor] nvarchar(20) NOT NULL,
    [CNPJFornecedor] nvarchar(18) NOT NULL,
    CONSTRAINT [PK_Fornecedores] PRIMARY KEY ([idFornecedor])
);

CREATE TABLE [Servicos] (
    [IdServico] int NOT NULL IDENTITY,
    [TipoServico] nvarchar(max) NOT NULL,
    [DescricaoServico] nvarchar(max) NOT NULL,
    [Preco] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Servicos] PRIMARY KEY ([IdServico])
);

CREATE TABLE [pedidos] (
    [idPedido] int NOT NULL IDENTITY,
    [idClienteId] int NOT NULL,
    [CupomId] int NULL,
    [DataPedido] datetime2 NOT NULL,
    [Subtotal] decimal(18,2) NOT NULL,
    [Desconto] decimal(18,2) NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_pedidos] PRIMARY KEY ([idPedido]),
    CONSTRAINT [FK_pedidos_Clientes_idClienteId] FOREIGN KEY ([idClienteId]) REFERENCES [Clientes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_pedidos_Cupons_CupomId] FOREIGN KEY ([CupomId]) REFERENCES [Cupons] ([idCupom])
);

CREATE TABLE [Produtos] (
    [IdProduto] int NOT NULL IDENTITY,
    [FornecedorId] int NOT NULL,
    [NomeProduto] nvarchar(200) NOT NULL,
    [Preco] decimal(18,2) NOT NULL,
    [Unidade] nvarchar(50) NOT NULL,
    [EstoqueProduto] int NOT NULL,
    [CodigoBarras] nvarchar(50) NOT NULL,
    [Peso] decimal(10,3) NOT NULL,
    [DescricaoProduto] nvarchar(500) NOT NULL,
    CONSTRAINT [PK_Produtos] PRIMARY KEY ([IdProduto]),
    CONSTRAINT [FK_Produtos_Fornecedores_FornecedorId] FOREIGN KEY ([FornecedorId]) REFERENCES [Fornecedores] ([idFornecedor]) ON DELETE NO ACTION
);

CREATE TABLE [Pagamentos] (
    [Id] int NOT NULL IDENTITY,
    [PedidoId] int NOT NULL,
    [ValorPagamento] decimal(18,2) NOT NULL,
    [DataPagamento] datetime2 NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [Discriminator] nvarchar(13) NOT NULL,
    [CodigoBarras] nvarchar(max) NULL,
    [linhaDigitavel] nvarchar(max) NULL,
    [DataVencimento] datetime2 NULL,
    [NomeTitular] nvarchar(max) NULL,
    [bandeira] nvarchar(max) NULL,
    [numeroCartao] nvarchar(max) NULL,
    [parcelas] int NULL,
    CONSTRAINT [PK_Pagamentos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Pagamentos_pedidos_PedidoId] FOREIGN KEY ([PedidoId]) REFERENCES [pedidos] ([idPedido]) ON DELETE CASCADE
);

CREATE TABLE [itemPedidos] (
    [Id] int NOT NULL IDENTITY,
    [PedidoId] int NOT NULL,
    [ProdutoId] int NULL,
    [ServicoId] int NULL,
    [Quantidade] int NOT NULL,
    [PrecoUnitario] decimal(18,2) NOT NULL,
    [Subtotal] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_itemPedidos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_itemPedidos_Produtos_ProdutoId] FOREIGN KEY ([ProdutoId]) REFERENCES [Produtos] ([IdProduto]),
    CONSTRAINT [FK_itemPedidos_Servicos_ServicoId] FOREIGN KEY ([ServicoId]) REFERENCES [Servicos] ([IdServico]),
    CONSTRAINT [FK_itemPedidos_pedidos_PedidoId] FOREIGN KEY ([PedidoId]) REFERENCES [pedidos] ([idPedido]) ON DELETE CASCADE
);

CREATE UNIQUE INDEX [IX_Clientes_CNPJ] ON [Clientes] ([CNPJ]) WHERE [CNPJ] IS NOT NULL;

CREATE UNIQUE INDEX [IX_Clientes_CPF] ON [Clientes] ([CPF]) WHERE [CPF] IS NOT NULL;

CREATE UNIQUE INDEX [IX_Clientes_Email] ON [Clientes] ([Email]);

CREATE UNIQUE INDEX [IX_Cupons_CodigoCupom] ON [Cupons] ([CodigoCupom]);

CREATE UNIQUE INDEX [IX_Fornecedores_CNPJFornecedor] ON [Fornecedores] ([CNPJFornecedor]);

CREATE INDEX [IX_itemPedidos_PedidoId] ON [itemPedidos] ([PedidoId]);

CREATE INDEX [IX_itemPedidos_ProdutoId] ON [itemPedidos] ([ProdutoId]);

CREATE INDEX [IX_itemPedidos_ServicoId] ON [itemPedidos] ([ServicoId]);

CREATE INDEX [IX_Pagamentos_PedidoId] ON [Pagamentos] ([PedidoId]);

CREATE INDEX [IX_pedidos_CupomId] ON [pedidos] ([CupomId]);

CREATE INDEX [IX_pedidos_idClienteId] ON [pedidos] ([idClienteId]);

CREATE UNIQUE INDEX [IX_Produtos_CodigoBarras] ON [Produtos] ([CodigoBarras]);

CREATE INDEX [IX_Produtos_FornecedorId] ON [Produtos] ([FornecedorId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261111221535_CriarTabelaProdutos', N'10.0.14');

COMMIT;
GO

