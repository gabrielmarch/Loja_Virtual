using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    public class Cartao
    {

        public Pagamento idPagamento { get; set; }

        public int Id { get; set; }

        public string NomeTitular { get; set; } = string.Empty;

        public string bandeira { get; set; } = string.Empty;

        public string numeroCartao { get; set; } = string.Empty;

        public int parcelas { get; set; }



    }
}
