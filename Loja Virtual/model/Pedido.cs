using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    public class Pedido
    {

        public int idPedido { get; set; }

        public int ClienteId { get; set; }

        public Cliente Cliente { get; set; } = null!;

        public int? CupomId { get; set; }

        public Cupom? Cupom { get; set; }

        public DateTime DataPedido { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Desconto { get; set; }

        public string Status { get; set; } = string.Empty;
        
        public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
        public Pagamento? Pagamento { get; set; }

    }
}
