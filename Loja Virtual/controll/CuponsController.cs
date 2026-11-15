
using Loja_Virtual.data;
using Loja_Virtual.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Loja_Virtual.controll
{
    public class CuponsController
    {
        // Lista os cupons para a tela de cupons
        public List<Cupom> ListarCupons()
        {
            using (var contexto = new LojaVirtualDbContext())
            {
                return contexto.cupoms
                    .AsNoTracking()
                    .OrderByDescending(c => c.Id)
                    .ToList();
            }
        }

        // Busca e valida um cupom para o carrinho
        public Cupom? BuscarCupomValido(string codigo)
        {
            using (var contexto = new LojaVirtualDbContext())
            {
                return contexto.cupoms
                    .AsNoTracking()
                    .FirstOrDefault(c =>
                        c.CodigoCupom == codigo &&
                        c.ativo &&
                        c.DataValidade >= DateTime.Today);
            }
        }
    }
}