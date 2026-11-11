using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    public class Pedido
    {

        public int Id { get; set; }

        public int ClienteId { get; set; }

        public Cliente Cliente { get; set; } = null!;

        public Cupom idCupom { get; set; }

        public DateTime DataPedido { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Desconto { get; set; }

        public string Status { get; set; } = string.Empty;

    }
}
