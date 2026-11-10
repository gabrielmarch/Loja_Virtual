using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    public class Boleto
    {

        private Pagamento idPagamento { get; set; }

        private string CodigoBarras { get; set; } = string.Empty;

        private string linhaDigitavel { get; set; } = string.Empty;

        private DateTime DataVencimento { get; set; }

    }
}
