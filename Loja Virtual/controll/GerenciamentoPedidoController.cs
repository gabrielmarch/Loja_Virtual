
using Loja_Virtual.data;
using Loja_Virtual.services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loja_Virtual.controll
{
    public class PedidoAdminExibicao
    {
        public int IdPedido { get; set; }
        public string NomeCliente { get; set; } = "";
        public DateTime DataPedido { get; set; }
        public decimal Total { get; set; }
        public string StatusPagamento { get; set; } = "";
        public string EtapaPedido { get; set; } = "";

        public string EtapaFormatada => EtapaPedido switch
        {
            "PedidoFeito" => "Pedido feito",
            "Produzindo" => "Produzindo",
            "Enviado" => "Enviado",
            "Entregue" => "Entregue",
            _ => "Não definida"
        };
    }

    public class GerenciamentoPedidoController
    {
        private static async Task VerificarAdministradorAsync(
            LojaVirtualDbContext banco)
        {
            if (!SessaoUsuario.EhAdministrador ||
                SessaoUsuario.ClienteId == null)
            {
                throw new UnauthorizedAccessException(
                    "Acesso permitido apenas para administradores.");
            }

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

        public async Task<List<PedidoAdminExibicao>>
            ListarPedidosAsync()
        {
            using var banco = new LojaVirtualDbContext();

            await VerificarAdministradorAsync(banco);

            return await banco.pedidos
                .AsNoTracking()
                .OrderByDescending(p => p.DataPedido)
                .Select(p => new PedidoAdminExibicao
                {
                    IdPedido = p.idPedido,
                    NomeCliente = p.idCliente.Nome,
                    DataPedido = p.DataPedido,
                    Total = p.Subtotal - p.Desconto,
                    StatusPagamento = p.Status,
                    EtapaPedido = p.EtapaPedido
                })
                .ToListAsync();
        }

        public async Task<string> AvancarEtapaAsync(int pedidoId)
        {
            using var banco = new LojaVirtualDbContext();

            await VerificarAdministradorAsync(banco);

            var pedido = await banco.pedidos
                .AsNoTracking()
                .Where(p => p.idPedido == pedidoId)
                .Select(p => new
                {
                    p.Status,
                    p.EtapaPedido
                })
                .FirstOrDefaultAsync();

            if (pedido == null)
            {
                throw new InvalidOperationException(
                    "Pedido não encontrado.");
            }
            if (pedido.Status != "Pago")
            {
                throw new InvalidOperationException(
                    "O pagamento precisa estar confirmado " +
                    "antes de avançar o pedido.");
            }

            string proximaEtapa = pedido.EtapaPedido switch
            {
                "PedidoFeito" => "Produzindo",
                "Produzindo" => "Enviado",
                "Enviado" => "Entregue",
                "Entregue" => throw new InvalidOperationException(
                    "Este pedido já foi entregue."),
                _ => throw new InvalidOperationException(
                    "A etapa atual do pedido é inválida.")
            };

            int alterados = await banco.pedidos
                .Where(p =>
                    p.idPedido == pedidoId &&
                    p.Status == "Pago" &&
                    p.EtapaPedido == pedido.EtapaPedido)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(
                        p => p.EtapaPedido,
                        proximaEtapa));

            if (alterados != 1)
            {
                throw new InvalidOperationException(
                    "O pedido foi alterado por outra operação. " +
                    "Atualize a lista antes de tentar novamente.");
            }

            return proximaEtapa;
        }
    }
}
