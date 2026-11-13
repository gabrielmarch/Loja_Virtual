
using Loja_Virtual.model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Loja_Virtual.model
{
    public class ItemCarrinho : INotifyPropertyChanged
    {
        public int IdProduto { get; }
        public string Nome { get; }
        public decimal Preco { get; }

        public int EstoqueDisponivel { get; }

        private int quantidade = 1;

        public int Quantidade
        {
            get => quantidade;

            set
            {
                int novaQuantidade = Math.Clamp(
                    value, 1, EstoqueDisponivel);

                if (quantidade == novaQuantidade)
                    return;

                quantidade = novaQuantidade;

                OnPropertyChanged();
                OnPropertyChanged(nameof(Subtotal));
            }
        }

        public decimal Subtotal => Preco * Quantidade;

        public ItemCarrinho(Produto produto)
        {
            IdProduto = produto.IdProduto;
            Nome = produto.NomeProduto;
            Preco = produto.Preco;
            EstoqueDisponivel = produto.EstoqueProduto;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(
            [CallerMemberName] string? propriedade = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propriedade));
        }
    }
}
