using Loja_Virtual.model;
using Loja_Virtual.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Loja_Virtual.data
{
    public static class DbInitializer
    {
        public static void Inicializar()
        {
            using var banco = new LojaVirtualDbContext();

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


            var fornecedor = banco.fornecedores
                .First(f =>
                    f.CNPJFornecedor == "00000000000001");

            AdicionarProdutoSeNaoExistir(
                banco,
                "7891234567891",
                "Mouse USB UVV",
                49.90m,
                20,
                fornecedor.idFornecedor,
                "mouse-usb.png");

            AdicionarProdutoSeNaoExistir(
                banco,
                "7891234568892",
                "Teclado USB",
                89.90m,
                20,
                fornecedor.idFornecedor,
                "teclado-usb.png");

            AdicionarProdutoSeNaoExistir(
                banco,
                "7891234598593",
                "Headset Sem Fio",
                159.90m,
                15,
                fornecedor.idFornecedor,
                "headset.png");

            AdicionarProdutoSeNaoExistir(
                banco,
                "7891234598573",
                "Mouse Gamer Sem Fio",
                109.90m,
                12,
                fornecedor.idFornecedor,
                "mouse-gamer.png");

            AdicionarProdutoSeNaoExistir(
                banco,
                "7891235555579",
                "PC Gamer Completo Ryzen 7",
                1959.90m,
                5,
                fornecedor.idFornecedor,
                "pc-ryzen7.png");

            AdicionarProdutoSeNaoExistir(
                banco,
                "7891235052578",
                "PC Gamer Completo Ryzen 5",
                1059.90m,
                4,
                fornecedor.idFornecedor,
                "pc-ryzen5.png");

            AdicionarProdutoSeNaoExistir(
                banco,
                "7891235252579",
                "Gabinete UVVirtual edição limitada",
                505.90m,
                2,
                fornecedor.idFornecedor,
                "gabinete.png");


            AdicionarCupomSeNaoExistir(
                banco, "DESCONTO10", 10m);

            AdicionarCupomSeNaoExistir(
                banco, "UVV15", 15m);

            AdicionarCupomSeNaoExistir(
                banco, "DESCONTO20", 20m);

            banco.SaveChanges();

            CriarAdministradorInicial(banco);
        }

        private static void AdicionarProdutoSeNaoExistir(
            LojaVirtualDbContext banco,
            string codigo,
            string nome,
            decimal preco,
            int estoque,
            int fornecedorId,
            string nomeImagem)
        {
            var produtoExistente = banco.produtos
                .FirstOrDefault(p => p.CodigoBarras == codigo);

            string caminhoImagem = Path.Combine(
                AppContext.BaseDirectory,
                "ImagensProdutos",
                nomeImagem);

            if (!File.Exists(caminhoImagem))
            {
                throw new FileNotFoundException(
                    $"Imagem não encontrada: {caminhoImagem}");
            }

            byte[] imagem = File.ReadAllBytes(caminhoImagem);

            if (produtoExistente != null)
            {
                if (produtoExistente.ImagemProduto == null ||
                    produtoExistente.ImagemProduto.Length == 0)
                {
                    produtoExistente.ImagemProduto = imagem;
                }

                return;
            }

            banco.produtos.Add(new Produto
            {
                NomeProduto = nome,
                CodigoBarras = codigo,
                Preco = preco,
                EstoqueProduto = estoque,
                FornecedorId = fornecedorId,
                Unidade = "UN",
                Peso = 0.300m,
                DescricaoProduto =
                    "Produto fictício para testes acadêmicos.",
                ImagemProduto = imagem
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


        private static void CriarAdministradorInicial(
            LojaVirtualDbContext banco)
        {
            const string cpfAdmin = "12345678900";
            const string emailAdmin = "admin@lojavirtual.test";
            const string senhaAdmin = "Admin@12345";

            bool adminExiste = banco.PessoasFisicas.Any(
                p => p.CPF == cpfAdmin ||
                     p.Email == emailAdmin);

            if (adminExiste)
                return;

            var administrador = new PessoaFisica
            {
                Nome = "Administrador 01",
                Email = emailAdmin,
                CPF = cpfAdmin,
                Telefone = "27999999999",
                Endereco = "Endereço de demonstração",
                DataNascimento = new DateTime(2000, 1, 1),
                DataCadastro = DateTime.Now,
                Ativo = true,
                PerfilAcesso = "Administrador",

                SenhaHash = SenhaService.GerarHash(senhaAdmin)
            };

            banco.PessoasFisicas.Add(administrador);
            banco.SaveChanges();
        }




    }
}
