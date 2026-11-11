using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    public class ItemPedido
    {
 
        public int Id { get; set; }

        public Pedido idPedido { get; set; }

        public Produto idProduto { get; set; }

        public Servico idServico { get; set; }

        public int Quantidade { get; set; }

        public decimal PrecoUnitario { get; set; }

        public decimal Subtotal { get; set; }

    }
}
