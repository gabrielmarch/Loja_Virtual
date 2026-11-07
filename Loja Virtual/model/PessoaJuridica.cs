using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    public class PessoaJuridica : Cliente
    {
        public string CNPJ { get; set; } = string.Empty;

        public string RazaoSocial { get; set; } = string.Empty;

        public string NomeFantasia { get; set; } = string.Empty;

        public bool ValidarCNPJ()
        {
            
            return false;
        }
    }
}
