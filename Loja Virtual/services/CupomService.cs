using System;
using System.Collections.Generic;
using System.Text;
using Loja_Virtual.data;
using Loja_Virtual.model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Loja_Virtual.services
{
    public static class CupomService
    {
        public static async Task<Cupom?> BuscarValidoAsync(
            LojaVirtualDbContext banco,
            string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                return null;

            string codigoNormalizado =
                codigo.Trim().ToUpperInvariant();

            DateTime hoje = DateTime.Today;

            return await banco.cupoms
                .AsNoTracking()
                .FirstOrDefaultAsync(c =>
                    c.CodigoCupom.ToUpper() == codigoNormalizado &&
                    c.ativo &&
                    c.DataValidade >= hoje &&
                    c.Desconto > 0m &&
                    c.Desconto <= 100m);
        }
    }
}
