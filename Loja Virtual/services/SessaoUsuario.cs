using System;
using System.Collections.Generic;
using System.Text;

using Loja_Virtual.model;

namespace Loja_Virtual.services
{
    public static class SessaoUsuario
    {
        public static int? ClienteId { get; private set; }

        public static string NomeCliente { get; private set; }
            = string.Empty;

        public static bool EstaAutenticado =>
            ClienteId.HasValue;

        public static void Iniciar(Cliente cliente)
        {
            ClienteId = cliente.Id;
            NomeCliente = cliente.Nome;
        }

        public static void Encerrar()
        {
            ClienteId = null;
            NomeCliente = string.Empty;
        }
    }
}
