using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    public class Pedido
    {
        public int Id { get; set; }

        // Cliente que realizou o pedido
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;

        // Cupom utilizado (opcional)
        public int? IdCupomId { get; set; }
        public Cupom? Cupom { get; set; }

        public DateTime DataPedido { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Desconto { get; set; }

        public string Status { get; set; } = string.Empty;

        // Itens pertencentes ao pedido
        public ICollection<ItemPedido> Itens { get; set; }
            = new List<ItemPedido>();

        // Pagamento do pedido
        public Pagamento? Pagamento { get; set; }

        // Valor final da compra
        public decimal ValorTotal => Subtotal - Desconto;
    }
}