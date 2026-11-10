using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Loja_Virtual.model
{
    internal class ItemCarrinho : INotifyPropertyChanged
    {
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        private int quantidade = 1;

        public int Quantidade
        {
            get => quantidade;
            set { if (value < 1)
                    value = 1;
                quantidade = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(Subtotal));
            }
        }
        public decimal Subtotal => Preco * Quantidade;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propriedade = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propriedade));
        }

        public ItemCarrinho(string nome, decimal preco)
        {
            Nome = nome;
            Preco = preco;
        }
    }
}
