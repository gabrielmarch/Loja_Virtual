using Loja_Virtual.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Loja_Virtual.controll
{
    internal class CarrinhoServiço
    {
        public static ObservableCollection<ItemCarrinho> Itens
        { get; } = new();

        public static decimal PercentualDesconto { get; private set; }

        public static decimal Subtotal => Itens.Sum(item => item.Subtotal);

        public static decimal Desconto => Subtotal * PercentualDesconto / 100;

        public static decimal Total => Subtotal - Desconto;

        public static void Adicionar(string nome, decimal preco)
        {
            var itemExistente = Itens.FirstOrDefault(item => item.Nome == nome && item.Preco == preco);

            if (itemExistente != null)
            {
                itemExistente.Quantidade++;
            }
            else
            {
                Itens.Add(new ItemCarrinho(nome, preco));
            }
        }

        public static bool AplicarCupom(string codigo)
        {
            PercentualDesconto = 0;

            if (codigo.ToUpper() == "DESCONTO10")
            {
                PercentualDesconto = 10;
                return true;
            }

            return false;
        }

        public static void Remover(ItemCarrinho item)
        {
            Itens.Remove(item);
        }

        public static void Limpar()
        {
            Itens.Clear();
            PercentualDesconto = 0;
        }

        public static void RemoverCupom()
        {
            PercentualDesconto = 0;
        }


    }
}
