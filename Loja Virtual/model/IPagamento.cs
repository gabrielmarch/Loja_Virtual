using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    public class Pagamento
    {

        private int idPagamento { get; set; }

        private decimal ValorPagamento { get; set; }

        private ItemPedido idItemPedido { get; set; }

        private DateTime DataPagamento { get; set; }

        private string status { get; set; } 

       

    }
}
