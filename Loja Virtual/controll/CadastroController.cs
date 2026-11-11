
using Loja_Virtual.data;
using Loja_Virtual.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

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

        public bool CadastrarCliente(
            string nome,
            string email,
            string cpfCnpj,
            string senha,
            out string mensagem)
        {
            string documento = new string(
                cpfCnpj.Where(char.IsDigit).ToArray());

            using var contexto = new LojaVirtualDbContext();

            bool documentoExiste = contexto.Clientes
                .OfType<PessoaFisica>()
                .Any(c => c.CPF == documento);

            documentoExiste = documentoExiste ||
                contexto.Clientes.OfType<PessoaJuridica>()
                .Any(c => c.CNPJ == documento);

            bool emailExiste = contexto.Clientes
                .Any(c => c.Email == email);

            if (documentoExiste || emailExiste)
            {
                mensagem = "CPF/CNPJ ou e-mail já cadastrado.";
                return false;
            }

            string hash = Convert.ToBase64String(
                SHA256.HashData(Encoding.UTF8.GetBytes(senha)));

            Cliente cliente;

            if (documento.Length == 11)
            {
                cliente = new PessoaFisica
                {
                    Nome = nome,
                    Email = email,
                    CPF = documento,
                    SenhaHash = hash,
                    DataCadastro = DateTime.Now,
                    Ativo = true
                };
            }
            else if (documento.Length == 14)
            {
                cliente = new PessoaJuridica
                {
                    Nome = nome,
                    Email = email,
                    CNPJ = documento,
                    RazaoSocial = nome,
                    NomeFantasia = nome,
                    SenhaHash = hash,
                    DataCadastro = DateTime.Now,
                    Ativo = true
                };
            }
            else
            {
                mensagem = "Informe um CPF ou CNPJ com quantidade válida de dígitos.";
                return false;
            }

            contexto.Clientes.Add(cliente);
            contexto.SaveChanges();

            mensagem = "Cliente cadastrado com sucesso!";
            return true;
        }
    }
}
