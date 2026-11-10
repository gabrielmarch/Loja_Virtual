using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    public class ItemPedido
    {
 
        private int idItemPedido { get; set; }

        private Pedido idPedido { get; set; }

        private Produto idProduto { get; set; }

        private Servico idServico { get; set; }

        private int Quantidade { get; set; }

        private decimal PrecoUnitario { get; set; }

        private decimal Subtotal { get; set; }

    }
}
