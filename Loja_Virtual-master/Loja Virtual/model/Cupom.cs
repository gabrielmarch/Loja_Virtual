using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    public class Cupom
    {

        private int idCupom { get; set; }

        private string CodigoCupom { get; set; } = string.Empty;

        private int Desconto { get; set; }

        private DateTime DataValidade { get; set; }

        private bool ativo { get; set; }

    }
}
