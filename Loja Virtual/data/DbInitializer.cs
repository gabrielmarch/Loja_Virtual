using System;
using System.Collections.Generic;
using System.Text;
using Loja_Virtual.model;
using System.Linq;

namespace Loja_Virtual.data
{
    public static class DbInitializer
    {
        public static void Inicializar()
        {
            using var banco = new LojaVirtualDbContext();

            // 1. FORNECEDORES DE DEMONSTRAÇÃO
            if (!banco.fornecedores.Any(f =>
                    f.CNPJFornecedor == "00000000000001"))
            {
                banco.fornecedores.Add(new Fornecedor
                {
                    NomeFornecedor = "Fornecedor Demo Tecnologia",
                    EmailFornecedor = "tecnologia@example.com",
                    TelefoneFornecedor = "27999990001",
                    CNPJFornecedor = "00000000000001"
                });
            }

            if (!banco.fornecedores.Any(f =>
                    f.CNPJFornecedor == "00000000000002"))
            {
                banco.fornecedores.Add(new Fornecedor
                {
                    NomeFornecedor = "Fornecedor Demo Acessorios",
                    EmailFornecedor = "acessorios@example.com",
                    TelefoneFornecedor = "27999990002",
                    CNPJFornecedor = "00000000000002"
                });
            }

            banco.SaveChanges();

            // Recuperar o fornecedor já cadastrado
            var fornecedor = banco.fornecedores
                .First(f =>
                    f.CNPJFornecedor == "00000000000001");

            // 2. PRODUTOS DE DEMONSTRAÇÃO
            AdicionarProdutoSeNaoExistir(
                banco,
                "DEMO-PROD-001",
                "Mouse USB Demonstracao",
                49.90m,
                30,
                fornecedor.idFornecedor);

            AdicionarProdutoSeNaoExistir(
                banco,
                "DEMO-PROD-002",
                "Teclado USB Demonstracao",
                89.90m,
                20,
                fornecedor.idFornecedor);

            AdicionarProdutoSeNaoExistir(
                banco,
                "DEMO-PROD-003",
                "Headset Demonstracao",
                159.90m,
                15,
                fornecedor.idFornecedor);

            // 3. CUPONS DE DEMONSTRAÇÃO
            AdicionarCupomSeNaoExistir(
                banco, "DESCONTO10", 10m);

            AdicionarCupomSeNaoExistir(
                banco, "DESCONTO15", 15m);

            AdicionarCupomSeNaoExistir(
                banco, "DESCONTO20", 20m);

            banco.SaveChanges();
        }

        private static void AdicionarProdutoSeNaoExistir(
            LojaVirtualDbContext banco,
            string codigo,
            string nome,
            decimal preco,
            int estoque,
            int fornecedorId)
        {
            if (banco.produtos.Any(p =>
                    p.CodigoBarras == codigo))
            {
                return;
            }

            banco.produtos.Add(new Produto
            {
                NomeProduto = nome,
                CodigoBarras = codigo,
                Preco = preco,
                EstoqueProduto = estoque,
                FornecedorId = fornecedorId,
                Unidade = "Unidade",
                Peso = 0.300m,
                DescricaoProduto =
                    "Produto fictício para testes acadêmicos.",
                ImagemProduto = null
            });
        }

        private static void AdicionarCupomSeNaoExistir(
            LojaVirtualDbContext banco,
            string codigo,
            decimal percentual)
        {
            if (banco.cupoms.Any(c =>
                    c.CodigoCupom == codigo))
            {
                return;
            }

            banco.cupoms.Add(new Cupom
            {
                CodigoCupom = codigo,
                Desconto = percentual,
                DataValidade = new DateTime(2035, 12, 31),
                ativo = true
            });
        }
    }
}
