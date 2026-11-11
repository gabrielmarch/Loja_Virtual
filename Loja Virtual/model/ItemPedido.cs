using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    public class ItemPedido
    {
 
        public int Id{ get; set; }

        public int PedidoId { get; set; }

        public Pedido Pedido { get; set; }

        public int? ProdutoId { get; set; }

        public Produto? Produto { get; set; }

        public int? ServicoId { get; set; }

        public Servico? Servico { get; set; }

        public int Quantidade { get; set; }

        public decimal PrecoUnitario { get; set; }

        public decimal Subtotal { get; set; }

    }
}
