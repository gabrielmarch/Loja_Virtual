using System;
using System.Collections.Generic;
using System.Text;

using Loja_Virtual.data;
using Loja_Virtual.model;
using Microsoft.EntityFrameworkCore;

namespace Loja_Virtual.controll
{
    public class ProdutoController
    {
        // LISTAR PRODUTOS DISPONÍVEIS
        public async Task<List<Produto>> ListarProdutosAsync()
        {
            using var banco = new LojaVirtualDbContext();

            return await banco.produtos
                .AsNoTracking()
                .Where(p => p.EstoqueProduto > 0)
                .OrderBy(p => p.NomeProduto)
                .ToListAsync();
        }

        // LISTAR FORNECEDORES
        public async Task<List<Fornecedor>> ListarFornecedoresAsync()
        {
            using var banco = new LojaVirtualDbContext();

            return await banco.fornecedores
                .AsNoTracking()
                .OrderBy(f => f.NomeFornecedor)
                .ToListAsync();
        }

        // CADASTRAR PRODUTO
        public async Task<int> CadastrarProdutoAsync(Produto produto)
        {
            // Validações
            if (string.IsNullOrWhiteSpace(produto.NomeProduto))
                throw new ArgumentException("Informe o nome do produto.");

            if (produto.NomeProduto.Length > 200)
                throw new ArgumentException("O nome deve ter até 200 caracteres.");

            if (produto.Preco <= 0)
                throw new ArgumentException("O preço deve ser maior que zero.");

            if (produto.EstoqueProduto < 0)
                throw new ArgumentException("O estoque não pode ser negativo.");

            if (produto.Peso < 0)
                throw new ArgumentException("O peso não pode ser negativo.");

            if (produto.FornecedorId <= 0)
                throw new ArgumentException("Selecione um fornecedor.");

            if (string.IsNullOrWhiteSpace(produto.CodigoBarras))
                throw new ArgumentException("Informe o código de barras.");

            if (produto.CodigoBarras.Length > 50)
                throw new ArgumentException("O código de barras deve ter até 50 caracteres.");

            if (produto.Unidade.Length > 50)
                throw new ArgumentException("A unidade deve ter até 50 caracteres.");

            if (produto.DescricaoProduto.Length > 500)
                throw new ArgumentException("A descrição deve ter até 500 caracteres.");

            using var banco = new LojaVirtualDbContext();

            // Verificar se o código de barras já existe
            bool codigoExiste = await banco.produtos
                .AnyAsync(p => p.CodigoBarras == produto.CodigoBarras);

            if (codigoExiste)
                throw new ArgumentException(
                    "Já existe um produto com esse código de barras.");

            // Verificar fornecedor
            bool fornecedorExiste = await banco.fornecedores
                .AnyAsync(f => f.idFornecedor == produto.FornecedorId);

            if (!fornecedorExiste)
                throw new ArgumentException(
                    "O fornecedor selecionado não existe.");

            // SALVAR NO BANCO
            banco.produtos.Add(produto);

            await banco.SaveChangesAsync();

            // Retorna o ID gerado pelo SQL Server
            return produto.IdProduto;
        }
    }
}
