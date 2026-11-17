using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using Loja_Virtual.data;
using Loja_Virtual.model;
using Loja_Virtual.services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Loja_Virtual.controll
{
    public class CadastroController
    {
        public async Task<int> CadastrarClienteAsync(
            string nome,
            string email,
            string documento,
            string telefone,
            string endereco,
            string senha,
            string confirmarSenha,
            DateTime? dataNascimento,
            string razaoSocial,
            string nomeFantasia)
        {
            nome = nome.Trim();
            email = email.Trim();
            telefone = telefone.Trim();
            endereco = endereco.Trim();

            documento = new string(
                documento.Where(char.IsDigit).ToArray());

            if (string.IsNullOrWhiteSpace(nome) || nome.Length > 150)
                throw new ArgumentException(
                    "Informe um nome válido de até 150 caracteres.");

            try
            {
                var emailValido = new MailAddress(email);

                if (emailValido.Address != email || email.Length > 100)
                    throw new Exception();
            }
            catch
            {
                throw new ArgumentException("Informe um e-mail válido.");
            }

            if (documento.Length != 11 && documento.Length != 14)
                throw new ArgumentException(
                    "Informe um CPF ou CNPJ com a quantidade correta de dígitos.");

            if (string.IsNullOrWhiteSpace(telefone) ||
                telefone.Length > 20)
                throw new ArgumentException("Informe um telefone válido.");

            if (string.IsNullOrWhiteSpace(endereco) ||
                endereco.Length > 200)
                throw new ArgumentException("Informe um endereço válido.");

            if (senha.Length < 8)
                throw new ArgumentException(
                    "A senha deve possuir pelo menos 8 caracteres.");

            if (senha != confirmarSenha)
                throw new ArgumentException("As senhas não coincidem.");

            using var banco = new LojaVirtualDbContext();

            bool emailExiste = await banco.Clientes
                .AnyAsync(c => c.Email == email);

            if (emailExiste)
                throw new ArgumentException(
                    "Este e-mail já está cadastrado.");

            Cliente cliente;

            if (documento.Length == 11)
            {
                bool cpfExiste = await banco.PessoasFisicas
                    .AnyAsync(c => c.CPF == documento);

                if (cpfExiste)
                    throw new ArgumentException(
                        "Este CPF já está cadastrado.");

                if (!dataNascimento.HasValue ||
                    dataNascimento.Value.Date > DateTime.Today ||
                    dataNascimento.Value.Date < new DateTime(1900, 1, 1))
                {
                    throw new ArgumentException(
                        "Informe uma data de nascimento válida.");
                }

                cliente = new PessoaFisica
                {
                    CPF = documento,
                    DataNascimento = dataNascimento.Value.Date
                };
            }
            else
            {
                bool cnpjExiste = await banco.PessoasJuridicas
                    .AnyAsync(c => c.CNPJ == documento);

                if (cnpjExiste)
                    throw new ArgumentException(
                        "Este CNPJ já está cadastrado.");

                if (string.IsNullOrWhiteSpace(razaoSocial) ||
                    razaoSocial.Length > 200 ||
                    nomeFantasia.Length > 200)
                {
                    throw new ArgumentException(
                        "Informe uma razão social válida.");
                }

                cliente = new PessoaJuridica
                {
                    CNPJ = documento,
                    RazaoSocial = razaoSocial.Trim(),
                    NomeFantasia = nomeFantasia.Trim()
                };
            }

            cliente.Nome = nome;
            cliente.Email = email;
            cliente.Telefone = telefone;
            cliente.Endereco = endereco;
            cliente.DataCadastro = DateTime.Now;
            cliente.Ativo = true;
            cliente.SenhaHash = SenhaService.GerarHash(senha);

            banco.Clientes.Add(cliente);

            await banco.SaveChangesAsync();

            return cliente.Id;
        }
    }
}
