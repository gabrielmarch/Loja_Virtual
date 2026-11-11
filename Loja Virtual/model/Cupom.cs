using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    public class Cupom
    {

        public int Id { get; set; }

        public string CodigoCupom { get; set; } = string.Empty;

        public int Desconto { get; set; }

        public DateTime DataValidade { get; set; }

        public bool ativo { get; set; }

    }
}
