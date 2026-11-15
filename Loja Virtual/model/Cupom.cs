using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Loja_Virtual.model
{
    public class Cupom
    {

        public int Id { get; set; }

        public string CodigoCupom { get; set; } = string.Empty;

        public int Desconto { get; set; }

        public DateTime DataValidade { get; set; }

        public bool ativo { get; set; }

        // Propriedades utilizadas somente na interface

        [NotMapped]
        public string Codigo => CodigoCupom;

        [NotMapped]
        public string Descricao => $"Aproveite {Desconto}% de desconto!";

        [NotMapped]
        public string DescontoTexto => $"{Desconto}% OFF";

        [NotMapped]
        public string ValidadeTexto =>
            $"Válido até {DataValidade:dd/MM/yyyy}";

        [NotMapped]
        public string StatusTexto =>
            !ativo ? "Inativo" :
            DataValidade.Date < DateTime.Today ? "Expirado" :
            "Ativo";

    }
}
