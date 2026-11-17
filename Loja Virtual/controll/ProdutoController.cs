using Loja_Virtual.data;
using Loja_Virtual.model;
using Loja_Virtual.services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.controll
{
    public class ProdutoController
    {
        public async Task<List<Produto>> ListarProdutosAsync()
        {
            using var banco = new LojaVirtualDbContext();

            return await banco.produtos
                .AsNoTracking()
                .Where(p => p.EstoqueProduto > 0)
                .OrderBy(p => p.NomeProduto)
                .ToListAsync();
        }

        public async Task<List<Fornecedor>> ListarFornecedoresAsync()
        {
            using var banco = new LojaVirtualDbContext();

            return await banco.fornecedores
                .AsNoTracking()
                .OrderBy(f => f.NomeFornecedor)
                .ToListAsync();
        }

        public async Task<int> CadastrarProdutoAsync(Produto produto)
        {

            if (!SessaoUsuario.EhAdministrador ||
                SessaoUsuario.ClienteId == null)
            {
                throw new UnauthorizedAccessException(
                    "Apenas administradores podem cadastrar produtos.");
            }

            using (var bancoPermissao = new LojaVirtualDbContext())
            {
                int usuarioId = SessaoUsuario.ClienteId.Value;

                bool autorizado = await bancoPermissao.Clientes
                    .AsNoTracking()
                    .AnyAsync(c =>
                        c.Id == usuarioId &&
                        c.Ativo &&
                        c.PerfilAcesso == "Administrador");

                if (!autorizado)
                {
                    throw new UnauthorizedAccessException(
                        "Sua conta não possui permissão administrativa.");
                }
            }

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

            bool codigoExiste = await banco.produtos
                .AnyAsync(p => p.CodigoBarras == produto.CodigoBarras);

            if (codigoExiste)
                throw new ArgumentException(
                    "Já existe um produto com esse código de barras.");

            bool fornecedorExiste = await banco.fornecedores
                .AnyAsync(f => f.idFornecedor == produto.FornecedorId);

            if (!fornecedorExiste)
                throw new ArgumentException(
                    "O fornecedor selecionado não existe.");

            banco.produtos.Add(produto);

            await banco.SaveChangesAsync();

            return produto.IdProduto;
        }
    }
}
