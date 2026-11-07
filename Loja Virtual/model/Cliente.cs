using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    public abstract class Cliente
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Telefone { get; set; } = string.Empty;

        public string Endereco { get; set; } = string.Empty;

        public DateTime DataCadastro { get; set; }

        public bool Ativo { get; set; }

        
        public string SenhaHash { get; set; } = string.Empty;

        public void AtualizarDados()
        {
            
        }

        public void ConsultarPedidos()
        {
            
        }
    }
}
