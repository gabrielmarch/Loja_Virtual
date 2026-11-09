
using Loja_Virtual.data;
using Loja_Virtual.model;
using System;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

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

                if (endereco.Address != email.Trim())
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

            string documento = Regex.Replace(cpfCnpj, @"\D", "");

            if (documento.Length != 11 && documento.Length != 14)
            {
                mensagem = "Informe um CPF com 11 números ou um CNPJ com 14 números.";
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

        
        public bool CadastrarCliente(
            string nome,
            string email,
            string cpfCnpj,
            string senha,
            string confirmarSenha,
            out string mensagem)
        {
            
            if (!ValidarCadastro(
                nome,
                email,
                cpfCnpj,
                senha,
                confirmarSenha,
                out mensagem))
            {
                return false;
            }

            string documento = Regex.Replace(cpfCnpj, @"\D", "");

            try
            {
                using (var db = new LojaVirtualDbContext())
                {
                    
                    bool emailExiste = db.Clientes.Any(c =>
                        c.Email == email.Trim());

                    if (emailExiste)
                    {
                        mensagem = "Este e-mail já está cadastrado.";
                        return false;
                    }

                    
                    bool documentoExiste =
                        db.PessoasFisicas.Any(p => p.CPF == documento) ||
                        db.PessoasJuridicas.Any(p => p.CNPJ == documento);

                    if (documentoExiste)
                    {
                        mensagem = "Este CPF/CNPJ já está cadastrado.";
                        return false;
                    }

                    
                    string senhaHash = GerarHashSenha(senha);

                    Cliente cliente;

                    
                    if (documento.Length == 11)
                    {
                        cliente = new PessoaFisica
                        {
                            CPF = documento,
                            DataNascimento = null
                        };
                    }
                    else
                    {
                        cliente = new PessoaJuridica
                        {
                            CNPJ = documento,
                            RazaoSocial = nome,
                            NomeFantasia = nome
                        };
                    }

                    
                    cliente.Nome = nome.Trim();
                    cliente.Email = email.Trim();
                    cliente.SenhaHash = senhaHash;
                    cliente.DataCadastro = DateTime.Now;
                    cliente.Ativo = true;

                    
                    db.Clientes.Add(cliente);

                    
                    db.SaveChanges();

                    mensagem = "Cadastro realizado com sucesso!";
                    return true;
                }
            }
            catch (Exception)
            {
                mensagem = "Não foi possível realizar o cadastro. Verifique a conexão com o banco de dados.";
                return false;
            }
        }

        
        private string GerarHashSenha(string senha)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                senha,
                salt,
                600000,
                HashAlgorithmName.SHA256,
                32
            );

            return $"PBKDF2$600000${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        }
    }
}
