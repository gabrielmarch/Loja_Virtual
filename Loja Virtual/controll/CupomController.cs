using System;
using System.Collections.Generic;
using System.Text;
using Loja_Virtual.data;
using Loja_Virtual.model;
using Loja_Virtual.services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Loja_Virtual.controll
{
    public class CupomController
    {
        private readonly ICupomRepository _repositorio;

        public CupomController() : this(new CupomRepositoryAdo())
        {
        }

        public CupomController(ICupomRepository repositorio)
        {
            _repositorio = repositorio;
        }

        private static async Task VerificarAdministradorAsync()
        {
            if (!SessaoUsuario.EhAdministrador ||
                SessaoUsuario.ClienteId == null)
            {
                throw new UnauthorizedAccessException(
                    "Apenas administradores podem gerenciar cupons.");
            }

            using var banco = new LojaVirtualDbContext();

            int usuarioId = SessaoUsuario.ClienteId.Value;

            bool autorizado = await banco.Clientes
                .AsNoTracking()
                .AnyAsync(c =>
                    c.Id == usuarioId &&
                    c.Ativo &&
                    c.PerfilAcesso == "Administrador");

            if (!autorizado)
            {
                throw new UnauthorizedAccessException(
                    "Sua conta não possui permissão administrativa.");
            }
        }

        private static void ValidarCupom(Cupom cupom)
        {
            if (string.IsNullOrWhiteSpace(cupom.CodigoCupom))
                throw new ArgumentException(
                    "Informe o código do cupom.");

            if (cupom.CodigoCupom.Length > 50)
                throw new ArgumentException(
                    "O código deve ter no máximo 50 caracteres.");

            if (cupom.Desconto <= 0m || cupom.Desconto > 100m)
                throw new ArgumentException(
                    "O desconto deve ser maior que 0 e até 100%.");

            if (cupom.DataValidade.Date < DateTime.Today)
                throw new ArgumentException(
                    "A data de validade não pode estar no passado.");
        }

        public async Task<List<Cupom>> ListarAsync()
        {
            await VerificarAdministradorAsync();

            return await _repositorio.ListarAsync();
        }

        public async Task<int> CadastrarAsync(Cupom cupom)
        {
            await VerificarAdministradorAsync();

            cupom.CodigoCupom =
                cupom.CodigoCupom.Trim().ToUpperInvariant();

            ValidarCupom(cupom);

            return await _repositorio.CadastrarAsync(cupom);
        }

        public async Task<bool> AtualizarAsync(Cupom cupom)
        {
            await VerificarAdministradorAsync();

            if (cupom.idCupom <= 0)
                throw new ArgumentException(
                    "Selecione um cupom para editar.");

            cupom.CodigoCupom =
                cupom.CodigoCupom.Trim().ToUpperInvariant();

            ValidarCupom(cupom);

            return await _repositorio.AtualizarAsync(cupom);
        }

        public async Task<bool> AlterarStatusAsync(
            int idCupom, bool ativo)
        {
            await VerificarAdministradorAsync();

            if (idCupom <= 0)
                throw new ArgumentException(
                    "Selecione um cupom válido.");

            return await _repositorio.AlterarStatusAsync(
                idCupom, ativo);
        }
    }
}
