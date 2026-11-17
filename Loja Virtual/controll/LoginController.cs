using System;
using System.Collections.Generic;
using System.Text;
using Loja_Virtual.data;
using Loja_Virtual.model;
using Loja_Virtual.services;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Loja_Virtual.controll
{
    public class LoginController
    {
        public async Task<Cliente?> AutenticarAsync(
            string cpfCnpj,
            string senha)
        {
            string documento = new string(
                cpfCnpj.Where(char.IsDigit).ToArray());

            if (documento.Length != 11 &&
                documento.Length != 14)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(senha))
                return null;

            using var banco = new LojaVirtualDbContext();

            Cliente? cliente = null;

            
            if (documento.Length == 11)
            {
                cliente = await banco.PessoasFisicas
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c =>
                        c.CPF == documento && c.Ativo);
            }
            
            else
            {
                cliente = await banco.PessoasJuridicas
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c =>
                        c.CNPJ == documento && c.Ativo);
            }

            if (cliente == null)
                return null;

            
            bool senhaCorreta = SenhaService.VerificarSenha(
                senha,
                cliente.SenhaHash);

            return senhaCorreta ? cliente : null;
        }
    }
}
