using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    internal class IPagamento
    {

        private int Id { get; set; }

        private decimal ValorPagamento { get; set; }

        private ItemPedido idItemPedido { get; set; }

        private DateTime DataPagamento { get; set; }

        private string status { get; set; } 

       

    }
}
