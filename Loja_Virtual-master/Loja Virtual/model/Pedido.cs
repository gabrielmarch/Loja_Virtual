using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    public class Pedido
    {

        private int idPedido { get; set; }

        private Cliente idCliente { get; set; }

        private Cupom idCupom { get; set; }

        private DateTime DataPedido { get; set; }

        private decimal Subtotal { get; set; }

        private decimal Desconto { get; set; }

        private string Status { get; set; } = string.Empty;

    }
}
