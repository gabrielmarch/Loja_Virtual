using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    public class Cartao
    {

        private Pagamento idPagamento { get; set; }

        private string NomeTitular { get; set; } = string.Empty;

        private string bandeira { get; set; } = string.Empty;

        private string numeroCartao { get; set; } = string.Empty;

        private int parcelas { get; set; }



    }
}
