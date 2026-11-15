
USE LojaVirtualDb;
GO

DECLARE @Quantidade INT = 0;
DECLARE @Codigo NVARCHAR(7);
DECLARE @Letras VARCHAR(26) = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';

WHILE @Quantidade < 15
BEGIN
    -- Gera 4 letras maiúsculas e 3 números
    SET @Codigo =
        SUBSTRING(@Letras, (CONVERT(BIGINT, CHECKSUM(NEWID())) & 2147483647) % 26 + 1, 1) +
        SUBSTRING(@Letras, (CONVERT(BIGINT, CHECKSUM(NEWID())) & 2147483647) % 26 + 1, 1) +
        SUBSTRING(@Letras, (CONVERT(BIGINT, CHECKSUM(NEWID())) & 2147483647) % 26 + 1, 1) +
        SUBSTRING(@Letras, (CONVERT(BIGINT, CHECKSUM(NEWID())) & 2147483647) % 26 + 1, 1) +
        CAST((CONVERT(BIGINT, CHECKSUM(NEWID())) & 2147483647) % 10 AS VARCHAR(1)) +
        CAST((CONVERT(BIGINT, CHECKSUM(NEWID())) & 2147483647) % 10 AS VARCHAR(1)) +
        CAST((CONVERT(BIGINT, CHECKSUM(NEWID())) & 2147483647) % 10 AS VARCHAR(1));

    -- Verifica se o código já existe
    IF NOT EXISTS (
        SELECT 1
        FROM dbo.cupoms
        WHERE CodigoCupom = @Codigo
    )
    BEGIN
        INSERT INTO dbo.cupoms
            (CodigoCupom, Desconto, DataValidade, ativo)
        VALUES
            (
                @Codigo,
                ((@Quantidade % 5) + 1) * 5,
                DATEADD(DAY, 30, GETDATE()),
                1
            );

        SET @Quantidade = @Quantidade + 1;
    END
END;

-- Exibe os 15 cupons cadastrados nesta execução
SELECT TOP 15
    Id,
    CodigoCupom,
    Desconto,
    DataValidade,
    ativo
FROM dbo.cupoms
ORDER BY Id DESC;
