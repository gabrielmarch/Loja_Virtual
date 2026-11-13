using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Loja_Virtual.controll;
using Loja_Virtual.model;

namespace Loja_Virtual.view
{
    /// <summary>
    /// Lógica interna para CarrinhoCompras.xaml
    /// </summary>
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
            ValorSubtotal.Text = CarrinhoServiço.Subtotal.ToString("C2");

            ValorDesconto.Text = CarrinhoServiço.Desconto.ToString("C2");

            ValorTotal.Text = CarrinhoServiço.Total.ToString("C2");
        }

        private void RemoverProduto_Click(object sender, RoutedEventArgs e)
        {
            Button botao = (Button)sender;

            ItemCarrinho item = (ItemCarrinho)botao.DataContext;

            CarrinhoServiço.Remover(item);

            AtualizarResumo();
        }


        private void AlterarQuantidade_Click(object sender, RoutedEventArgs e)
        {
            Button botao = (Button)sender;

            ItemCarrinho item = (ItemCarrinho)botao.DataContext;

            if (botao.Tag.ToString() == "Aumentar")
            {
                item.Quantidade++;
            }
            else if (botao.Tag.ToString() == "Diminuir")
            {
                if (item.Quantidade > 1)
                {
                    item.Quantidade--;
                }
            }

            AtualizarResumo();
        }


        private void AplicarCupom_Click(object sender, RoutedEventArgs e)
        {
            bool aplicado = CarrinhoServiço.AplicarCupom(Cupom.Text);

            if (aplicado)
            {
                MensagemCupom.Text = "Cupom aplicado! 10% de desconto.";
                MensagemCupom.Foreground = Brushes.Green;
            }
            else
            {
                CarrinhoServiço.RemoverCupom();

                MensagemCupom.Text = "Cupom inválido!";
                MensagemCupom.Foreground = Brushes.Red;
            }

            AtualizarResumo();
        }


        private void Pagamento_Click(object sender, RoutedEventArgs e)
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



        private void button_VoltarCarHome_Click(object sender, RoutedEventArgs e)
        {
            TelaPrincipal telaPrincipal = new TelaPrincipal();
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
