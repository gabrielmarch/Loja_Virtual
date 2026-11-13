using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.model
{
    public class Produto
    {
        public int idProduto { get; set; }

        public Fornecedor idFornecedor { get; set; } = null!;

        public string NomeProduto { get; set; } = string.Empty;

        public int EstoqueProduto { get; set; }

        public string CodigoBarras { get; set; } = string.Empty;

        public decimal Peso { get; set; }

        public string DescricaoProduto { get; set; } = string.Empty;


    }
}
