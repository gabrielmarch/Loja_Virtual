using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    public class Cartao : Pagamento
    {

        public string NomeTitular { get; set; } = string.Empty;

        public string bandeira { get; set; } = string.Empty;

        public string numeroCartao { get; set; } = string.Empty;

        public int parcelas { get; set; }



    }
}
