
using Loja_Virtual.model;
using System.Collections.ObjectModel;
using System.Linq;
using Loja_Virtual.data;
using Loja_Virtual.services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Loja_Virtual.controll
{
    public static class CarrinhoServiço
    {
        public static ObservableCollection<ItemCarrinho> Itens
        { get; } = new();

        public static int? CupomId { get; private set; }

        public static string? CodigoCupom { get; private set; }

        public static decimal PercentualDesconto
        { get; private set; }

        public static decimal Subtotal =>
            Itens.Sum(item => item.Subtotal);

        public static decimal Desconto =>
            Math.Round(
                Subtotal * PercentualDesconto / 100m,
                2,
                MidpointRounding.AwayFromZero);

        public static decimal Total =>
            Subtotal - Desconto;

        public static bool Adicionar(Produto produto)
        {
            if (produto.EstoqueProduto <= 0)
                return false;

            var itemExistente = Itens.FirstOrDefault(
                item => item.IdProduto == produto.IdProduto);

            if (itemExistente != null)
            {
                if (itemExistente.Quantidade >=
                    itemExistente.EstoqueDisponivel)
                {
                    return false;
                }

                itemExistente.Quantidade++;
            }
            else
            {
                Itens.Add(new ItemCarrinho(produto));
            }

            return true;
        }
        public static void Remover(ItemCarrinho item)
        {
            Itens.Remove(item);
        }
        public static async Task<bool> AplicarCupomAsync(
            string codigo)
        {
            RemoverCupom();

            using var banco = new LojaVirtualDbContext();

            Cupom? cupom = await CupomService.BuscarValidoAsync(
                banco, codigo);

            if (cupom == null)
                return false;

            CupomId = cupom.idCupom;
            CodigoCupom = cupom.CodigoCupom;
            PercentualDesconto = cupom.Desconto;

            return true;
        }

        public static void RemoverCupom()
        {
            CupomId = null;
            CodigoCupom = null;
            PercentualDesconto = 0m;
        }

        public static void Limpar()
        {
            Itens.Clear();
            RemoverCupom();
        }
    }
}
