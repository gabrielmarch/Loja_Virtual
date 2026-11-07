using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.controll
{
    public class LoginController
    {
        public bool ValidarLogin(
            string cpfCnpj,
            string senha,
            out string mensagem)
        {
            if (string.IsNullOrWhiteSpace(cpfCnpj))
            {
                mensagem = "Informe seu CPF ou CNPJ.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(senha))
            {
                mensagem = "Informe sua senha.";
                return false;
            }

            mensagem = "Dados preenchidos corretamente.";
            return true;
        }
    }
}
