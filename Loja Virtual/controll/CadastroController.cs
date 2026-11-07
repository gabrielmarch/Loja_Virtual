using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace Loja_Virtual.controll
{
    public class CadastroController
    {
        public bool ValidarCadastro(
            string nome,
            string email,
            string cpfCnpj,
            string senha,
            string confirmarSenha,
            out string mensagem)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                mensagem = "Informe seu nome.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                mensagem = "Informe seu e-mail.";
                return false;
            }

            try
            {
                var endereco = new MailAddress(email);

                if (endereco.Address != email)
                {
                    mensagem = "Informe um e-mail válido.";
                    return false;
                }
            }
            catch
            {
                mensagem = "Informe um e-mail válido.";
                return false;
            }

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

            if (senha.Length < 6)
            {
                mensagem = "A senha deve possuir pelo menos 6 caracteres.";
                return false;
            }

            if (senha != confirmarSenha)
            {
                mensagem = "As senhas não coincidem.";
                return false;
            }

            mensagem = "Dados preenchidos corretamente.";
            return true;
        }
    }
}
