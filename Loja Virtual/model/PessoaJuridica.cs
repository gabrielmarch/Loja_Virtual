using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    public class PessoaJuridica : Cliente
    {
        private Cliente idCliente { get; set; }

        private string CNPJ { get; set; } = string.Empty;

        private string RazaoSocial { get; set; } = string.Empty;

        private string NomeFantasia { get; set; } = string.Empty;

        private bool ValidarCNPJ()
        {
            
            return false;
        }
    }
}
