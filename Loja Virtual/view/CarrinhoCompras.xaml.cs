using Loja_Virtual.model;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Loja_Virtual.services;

namespace Loja_Virtual.view
{
    public partial class CarrinhoCompras : Window
    {
        private bool menuVisivel = true;

        public CarrinhoCompras()
        {
            InitializeComponent();

            ListaCarrinho.ItemsSource = CarrinhoService.Itens;

            AtualizarResumo();
        }

        private void AtualizarResumo()
        {
            decimal subtotal = CarrinhoService.Itens.Sum(p => p.Subtotal);

            ValorSubtotal.Text = subtotal.ToString(
                "C2", new CultureInfo("pt-BR"));

            ValorTotal.Text = subtotal.ToString(
                "C2", new CultureInfo("pt-BR"));

            ValorDesconto.Text = 0m.ToString(
                "C2", new CultureInfo("pt-BR"));
        }

        private void RemoverProduto_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement elemento &&
                elemento.DataContext is ItemCarrinho item)
            {
                CarrinhoService.Remover(item);
                AtualizarResumo();
            }
        }

        private void RemoverProduto1_Click(object sender, RoutedEventArgs e)
        {
        }

        private void RemoverProduto2_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Quantidade_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void AplicarCupom_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Funcionalidade de cupom ainda não implementada.");
        }
        
        private void Pagamento_Click(object sender, RoutedEventArgs e)
        {
            MetodoPagamento metodoPagamento = new MetodoPagamento();

            metodoPagamento.Show();

            this.Close();
        }

        private void button_VoltarCarHome_Click(object sender, RoutedEventArgs e)
        {
            TelaPrincipal telaPrincipal =
            new TelaPrincipal(SessaoUsuario.ClienteId);

            telaPrincipal.Show();
            this.Close();
        }

        private void buttonMenu_Click(object sender, RoutedEventArgs e)
        {
            menuVisivel = !menuVisivel;

            if (menuVisivel)
            {
                BarraLateral.Visibility = Visibility.Visible;
                ColunaMenu.Width = new GridLength(165);
            }
            else
            {
                BarraLateral.Visibility = Visibility.Collapsed;
                ColunaMenu.Width = new GridLength(0);
            }
        }
    }
}
