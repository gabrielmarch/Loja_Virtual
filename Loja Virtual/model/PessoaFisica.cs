using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    public class PessoaFisica : Cliente
    {
        public string CPF { get; set; } = string.Empty;

        public DateTime DataNascimento { get; set; }

        public bool ValidarCPF()
        {
            
            return false;
        }
    }
}
