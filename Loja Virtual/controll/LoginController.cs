
using Loja_Virtual.data;
using Loja_Virtual.model;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Loja_Virtual.controll
{
    public class LoginController
    {
        public bool ValidarLogin(
            string cpfCnpj,
            string senha,
            out string mensagem)
        {
            if (string.IsNullOrWhiteSpace(cpfCnpj) &&
                string.IsNullOrWhiteSpace(senha))
            {
                mensagem = "Preencha o CPF/CNPJ e a senha.";
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

            string documento = Regex.Replace(cpfCnpj, @"\D", "");

            if (documento.Length != 11 && documento.Length != 14)
            {
                mensagem = "Informe um CPF com 11 números ou um CNPJ com 14 números.";
                return false;
            }

            mensagem = string.Empty;
            return true;
        }

        public bool Autenticar(
            string cpfCnpj,
            string senha,
            out string mensagem)
        {
            if (!ValidarLogin(cpfCnpj, senha, out mensagem))
            {
                return false;
            }

            string documento = Regex.Replace(cpfCnpj, @"\D", "");

            try
            {
                using (var db = new LojaVirtualDbContext())
                {
                    Cliente cliente;

                    if (documento.Length == 11)
                    {
                        cliente = db.PessoasFisicas.FirstOrDefault(
                            p => p.CPF == documento);
                    }
                    else
                    {
                        cliente = db.PessoasJuridicas.FirstOrDefault(
                            p => p.CNPJ == documento);
                    }

                    if (cliente == null || !cliente.Ativo)
                    {
                        mensagem = "Usuário não encontrado no sistema ou senha incorreta.";
                        return false;
                    }

                    if (!VerificarSenha(senha, cliente.SenhaHash))
                    {
                        mensagem = "Usuário não encontrado no sistema ou senha incorreta.";
                        return false;
                    }

                    mensagem = "Login realizado com sucesso!";
                    return true;
                }
            }
            catch (Exception)
            {
                mensagem = "Não foi possível acessar o banco de dados. Tente novamente.";
                return false;
            }
        }

        private bool VerificarSenha(string senha, string senhaHash)
        {
            try
            {
                string[] partes = senhaHash.Split('$');

                if (partes.Length != 4 || partes[0] != "PBKDF2")
                    return false;

                int iteracoes = int.Parse(partes[1]);

                byte[] salt = Convert.FromBase64String(partes[2]);
                byte[] hashOriginal = Convert.FromBase64String(partes[3]);

                byte[] hashDigitado = Rfc2898DeriveBytes.Pbkdf2(
                    senha,
                    salt,
                    iteracoes,
                    HashAlgorithmName.SHA256,
                    hashOriginal.Length
                );

                return CryptographicOperations.FixedTimeEquals(
                    hashOriginal,
                    hashDigitado
                );
            }
            catch
            {
                return false;
            }
        }
    }
}
