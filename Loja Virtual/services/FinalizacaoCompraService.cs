using System;
using System.Collections.Generic;
using System.Text;
using Loja_Virtual.model;
using System.Collections.Generic;
using System.Linq;

namespace Loja_Virtual.services
{
    public static class FinalizacaoCompraService
    {
        public static List<ItemCarrinho> Itens { get; private set; }
            = new List<ItemCarrinho>();

        public static int? CupomId { get; private set; }

        public static decimal PercentualDesconto { get; private set; }

        public static decimal Subtotal { get; private set; }

        public static decimal Desconto { get; private set; }

        public static decimal Total { get; private set; }

        public static bool CompraPreparada =>
            Itens.Count > 0;

        public static void PrepararCompra(
            IEnumerable<ItemCarrinho> itens,
            int? cupomId,
            decimal percentualDesconto)
        {
            Itens = itens.Select(item => new ItemCarrinho
            {
                Id = item.Id,
                Nome = item.Nome,
                Preco = item.Preco,
                Quantidade = item.Quantidade
            }).ToList();

            CupomId = cupomId;

            PercentualDesconto = percentualDesconto;

            Subtotal = Itens.Sum(item => item.Subtotal);

            Desconto = System.Math.Round(
                Subtotal * PercentualDesconto / 100m, 2);

            Total = Subtotal - Desconto;
        }

        public static void Limpar()
        {
            Itens.Clear();

            CupomId = null;
            PercentualDesconto = 0;
            Subtotal = 0;
            Desconto = 0;
            Total = 0;
        }
    }
}
