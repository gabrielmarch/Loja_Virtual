using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    public class Boleto
    {

        public int Id { get; set; }

        public Pagamento? Pagamento { get; set; }

        public string CodigoBarras { get; set; } = string.Empty;
            
        public string LinhaDigitavel { get; set; } = string.Empty;

        public DateTime DataVencimento { get; set; }

    }
}
