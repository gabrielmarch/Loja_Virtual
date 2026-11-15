using Loja_Virtual.controll;
using Loja_Virtual.model;
using Loja_Virtual.services;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Loja_Virtual.view
{
    public partial class CarrinhoCompras : Window
    {
        private bool menuVisivel = true;

        private readonly CuponsController _cuponsController =
            new CuponsController();

        private Cupom? cupomAplicado;

        private decimal percentualDesconto = 0m;

        private readonly CultureInfo cultura =
            new CultureInfo("pt-BR");

        public CarrinhoCompras()
        {
            InitializeComponent();

            ListaCarrinho.ItemsSource = CarrinhoService.Itens;

            AtualizarResumo();
        }

        private void AtualizarResumo()
        {
            decimal subtotal = CarrinhoService.Itens
                .Sum(p => p.Subtotal);

            decimal desconto = Math.Round(
                subtotal * percentualDesconto / 100m,
                2);

            decimal total = subtotal - desconto;

            ValorSubtotal.Text = subtotal.ToString("C2", cultura);

            ValorDesconto.Text = desconto.ToString("C2", cultura);

            ValorTotal.Text = total.ToString("C2", cultura);
        }

        private void RemoverProduto_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (sender is FrameworkElement elemento &&
                elemento.DataContext is ItemCarrinho item)
            {
                CarrinhoService.Remover(item);
                AtualizarResumo();
            }
        }

        private void RemoverProduto1_Click(
            object sender,
            RoutedEventArgs e)
        {
        }

        private void RemoverProduto2_Click(
            object sender,
            RoutedEventArgs e)
        {
        }

        private void Quantidade_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            AtualizarResumo();
        }

        private void AplicarCupom_Click(
            object sender,
            RoutedEventArgs e)
        {
            string codigo = Cupom.Text.Trim().ToUpperInvariant();

            // Verifica se o campo está vazio
            if (string.IsNullOrWhiteSpace(codigo))
            {
                MostrarMensagem(
                    "Digite o código do cupom.",
                    false);

                return;
            }

            // Valida o formato: 4 letras maiúsculas e 3 números
            if (!Regex.IsMatch(codigo, @"^[A-Z]{4}[0-9]{3}$"))
            {
                MostrarMensagem(
                    "Formato inválido. Utilize 4 letras e 3 números. Ex.: ABCD123.",
                    false);

                return;
            }

            // Verifica se existem produtos no carrinho
            decimal subtotal = CarrinhoService.Itens
                .Sum(p => p.Subtotal);

            if (subtotal <= 0)
            {
                MostrarMensagem(
                    "Adicione produtos ao carrinho antes de aplicar um cupom.",
                    false);

                return;
            }

            try
            {
                Cupom? cupom = _cuponsController
                    .BuscarCupomValido(codigo);

                if (cupom == null)
                {
                    MostrarMensagem(
                        "Cupom inexistente, inativo ou expirado.",
                        false);

                    return;
                }

                // Aplica o cupom
                cupomAplicado = cupom;
                percentualDesconto = cupom.Desconto;

                AtualizarResumo();

                MostrarMensagem(
                    $"Cupom {cupom.CodigoCupom} aplicado! Desconto de {cupom.Desconto}%.",
                    true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Não foi possível validar o cupom.\n\n" + ex.Message,
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void MostrarMensagem(string mensagem, bool sucesso)
        {
            MensagemCupom.Text = mensagem;

            MensagemCupom.Foreground = sucesso
                ? new SolidColorBrush(Color.FromRgb(38, 155, 112))
                : new SolidColorBrush(Color.FromRgb(217, 83, 96));
        }

        private void Pagamento_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (!CarrinhoService.Itens.Any())
            {
                MessageBox.Show(
                    "Adicione pelo menos um produto ao carrinho.",
                    "Carrinho vazio",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            
            FinalizacaoCompraService.PrepararCompra(
                CarrinhoService.Itens,
                cupomAplicado?.Id,
                percentualDesconto);

            MetodoPagamento metodoPagamento =
                new MetodoPagamento();

            metodoPagamento.Show();

            this.Close();
        }

        private void button_VoltarCarHome_Click(
            object sender,
            RoutedEventArgs e)
        {
            TelaPrincipal telaPrincipal =
                new TelaPrincipal(SessaoUsuario.ClienteId);

            telaPrincipal.Show();

            this.Close();
        }

        private void buttonMenu_Click(
            object sender,
            RoutedEventArgs e)
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