
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Loja_Virtual.controll;
using Loja_Virtual.model;

namespace Loja_Virtual.view
{
    public partial class CarrinhoCompras : Window
    {
        private bool menuVisivel = true;

        public CarrinhoCompras()
        {
            InitializeComponent();
            ListaCarrinho.ItemsSource = CarrinhoServiço.Itens;

            AtualizarResumo();
        }

        private void AtualizarResumo()
        {
            var cultura = CultureInfo.GetCultureInfo("pt-BR");

            ValorSubtotal.Text =
                CarrinhoServiço.Subtotal.ToString("C2", cultura);

            ValorDesconto.Text =
                CarrinhoServiço.Desconto.ToString("C2", cultura);

            ValorTotal.Text =
                CarrinhoServiço.Total.ToString("C2", cultura);
        }

        private void RemoverProduto_Click(
            object sender, RoutedEventArgs e)
        {
            if (sender is not Button botao ||
                botao.DataContext is not ItemCarrinho item)
                return;

            CarrinhoServiço.Remover(item);

            AtualizarResumo();
        }

        private void AlterarQuantidade_Click(
            object sender, RoutedEventArgs e)
        {
            if (sender is not Button botao ||
                botao.DataContext is not ItemCarrinho item)
                return;

            string acao = botao.Tag?.ToString() ?? "";

            if (acao == "Aumentar")
            {
                if (item.Quantidade >= item.EstoqueDisponivel)
                {
                    MessageBox.Show(
                        $"Estoque máximo disponível: {item.EstoqueDisponivel}",
                        "Estoque insuficiente",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                item.Quantidade++;
            }
            else if (acao == "Diminuir" && item.Quantidade > 1)
            {
                item.Quantidade--;
            }

            AtualizarResumo();
        }

        private async void AplicarCupom_Click(
            object sender, RoutedEventArgs e)
        {
            Button? botao = sender as Button;

            if (botao != null)
                botao.IsEnabled = false;

            try
            {
                bool aplicado =
                    await CarrinhoServiço.AplicarCupomAsync(
                        Cupom.Text.Trim());

                if (aplicado)
                {
                    MensagemCupom.Text =
                        $"Cupom aplicado! " +
                        $"{CarrinhoServiço.PercentualDesconto:0.##}% " +
                        "de desconto.";

                    MensagemCupom.Foreground = Brushes.Green;
                }
                else
                {
                    MensagemCupom.Text =
                        "Cupom inválido, vencido ou inativo.";

                    MensagemCupom.Foreground = Brushes.Red;
                }

                AtualizarResumo();
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Não foi possível consultar os cupons no banco.",
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                if (botao != null)
                    botao.IsEnabled = true;

                AtualizarResumo();
            }
        }



        private void Pagamento_Click(
            object sender, RoutedEventArgs e)
        {
            if (CarrinhoServiço.Itens.Count == 0)
            {
                MessageBox.Show(
                    "Adicione pelo menos um produto ao carrinho.",
                    "Carrinho vazio",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            MetodoPagamento pagamento =
                new MetodoPagamento(CarrinhoServiço.Total);

            pagamento.Show();
            this.Close();
        }

        private void button_VoltarCarHome_Click(
            object sender, RoutedEventArgs e)
        {
            TelaPrincipal principal = new TelaPrincipal();
            principal.Show();

            this.Close();
        }

        private void buttonMenu_Click(
            object sender, RoutedEventArgs e)
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
