using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    public class Produto
    {
        public int IdProduto { get; set; }

        public int FornecedorId { get; set; }

        public Fornecedor Fornecedor { get; set; } = null!;

        public string NomeProduto { get; set; } = string.Empty;

        public decimal Preco { get; set; }
        public string Unidade { get; set; } = string.Empty;

        public int EstoqueProduto { get; set; }

        public string CodigoBarras { get; set; } = string.Empty;

        public decimal Peso { get; set; }

        public string DescricaoProduto { get; set; } = string.Empty;


    }
}
