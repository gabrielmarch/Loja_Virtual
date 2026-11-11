using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    public class Servico
    {
        public int IdServico { get; set; }

        public string TipoServico { get; set; } = string.Empty;

        public string DescricaoServico { get; set; } = string.Empty;

        public decimal Preco { get; set; }


    }
}
