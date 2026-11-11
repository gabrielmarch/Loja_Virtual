using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    public class Fornecedor
    {

        public int idFornecedor { get; set; }

        public string NomeFornecedor { get; set; } = string.Empty;

        public string EmailFornecedor { get; set; } = string.Empty;

        public string TelefoneFornecedor { get; set; } = string.Empty;

        public string CNPJFornecedor { get; set; } = string.Empty;

        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
        public Fornecedor()
        {

        }

        public Fornecedor(int idFornecedor, string nomeFornecedor, string emailFornecedor, string telefoneFornecedor, string cNPJFornecedor)
        {
            this.idFornecedor = idFornecedor;
            NomeFornecedor = nomeFornecedor;
            EmailFornecedor = emailFornecedor;
            TelefoneFornecedor = telefoneFornecedor;
            CNPJFornecedor = cNPJFornecedor;
        }
    }
}
