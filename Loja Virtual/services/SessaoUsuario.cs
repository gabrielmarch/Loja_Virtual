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

        public static string PerfilAcesso { get; private set; }
            = "Cliente";

        public static bool EstaAutenticado =>
            ClienteId.HasValue;

        public static bool EhAdministrador =>
            EstaAutenticado &&
            PerfilAcesso == "Administrador";

        public static void Iniciar(Cliente cliente)
        {
            ClienteId = cliente.Id;
            NomeCliente = cliente.Nome;
            PerfilAcesso = cliente.PerfilAcesso;
        }

        public static void Encerrar()
        {
            ClienteId = null;
            NomeCliente = string.Empty;
            PerfilAcesso = "Cliente";
        }
    }
}
