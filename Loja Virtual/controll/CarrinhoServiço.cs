
using Loja_Virtual.model;
using System.Collections.ObjectModel;
using System.Linq;

namespace Loja_Virtual.controll
{
    public static class CarrinhoServiço
    {
        public static ObservableCollection<ItemCarrinho> Itens
        { get; } = new();

        public static decimal PercentualDesconto { get; private set; }

        public static decimal Subtotal =>
            Itens.Sum(item => item.Subtotal);

        public static decimal Desconto =>
            Subtotal * PercentualDesconto / 100;

        public static decimal Total =>
            Subtotal - Desconto;

        // ADICIONAR PRODUTO
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

        // REMOVER PRODUTO
        public static void Remover(ItemCarrinho item)
        {
            Itens.Remove(item);
        }

        // APLICAR CUPOM FICTÍCIO
        public static bool AplicarCupom(string codigo)
        {
            PercentualDesconto = 0;

            if (codigo.Trim().Equals(
                "DESCONTO10",
                StringComparison.OrdinalIgnoreCase))
            {
                PercentualDesconto = 10;
                return true;
            }

            return false;
        }

        public static void RemoverCupom()
        {
            PercentualDesconto = 0;
        }

        public static void Limpar()
        {
            Itens.Clear();
            PercentualDesconto = 0;
        }
    }
}
