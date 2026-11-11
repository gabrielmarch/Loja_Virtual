
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Loja_Virtual.data;
using Loja_Virtual.model;

namespace Loja_Virtual.controll
{
    public class LoginController
    {
        public bool ValidarLogin(
            string cpfCnpj,
            string senha,
            out string mensagem,
            out int clienteId)
        {
            clienteId = 0;

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

            string documento = new string(
                cpfCnpj.Where(char.IsDigit).ToArray()
            );

            string senhaHash = Convert.ToBase64String(
                SHA256.HashData(Encoding.UTF8.GetBytes(senha))
            );

            using (var contexto = new LojaVirtualDbContext())
            {
                Cliente? cliente = null;

                if (documento.Length == 11)
                {
                    cliente = contexto.Clientes
                        .OfType<PessoaFisica>()
                        .FirstOrDefault(c =>
                            c.CPF == documento &&
                            c.SenhaHash == senhaHash &&
                            c.Ativo);
                }
                else if (documento.Length == 14)
                {
                    cliente = contexto.Clientes
                        .OfType<PessoaJuridica>()
                        .FirstOrDefault(c =>
                            c.CNPJ == documento &&
                            c.SenhaHash == senhaHash &&
                            c.Ativo);
                }

                if (cliente == null)
                {
                    mensagem = "CPF/CNPJ ou senha incorretos.";
                    return false;
                }

                clienteId = cliente.Id;
            }

            mensagem = "Login realizado com sucesso!";
            return true;
        }
    }
}
