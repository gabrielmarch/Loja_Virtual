using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    public class Pagamento
    {
        public int Id { get; set; }

        public int PedidoId { get; set; }

        public Pedido Pedido { get; set; } = null!;

        public string MetodoPagamento { get; set; }
            = string.Empty;

        public decimal Valor { get; set; }

        public DateTime DataPagamento { get; set; }

        public string Status { get; set; }
            = string.Empty;
    }
}
