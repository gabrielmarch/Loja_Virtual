using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    public class Boleto : Pagamento
    {   

        public string CodigoBarras { get; set; } = string.Empty;

        public string linhaDigitavel { get; set; } = string.Empty;

        public DateTime DataVencimento { get; set; }

    }
}
