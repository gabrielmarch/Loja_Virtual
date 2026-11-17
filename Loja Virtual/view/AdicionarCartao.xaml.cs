using Loja_Virtual.controll;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Loja_Virtual.view
{
    public partial class AdicionarCartao : Window
    {
        private bool menuVisivel = true;
        private bool pagamentoConfirmado = false;
        public decimal ValorCompra { get; private set; }

        public AdicionarCartao() : this(0m)
        {
        }

        public AdicionarCartao(decimal valorCompra)
        {
            InitializeComponent();

            ValorCompra = valorCompra;

            ValorCompraTexto.Text = valorCompra.ToString(
                "C2",
                System.Globalization.CultureInfo.GetCultureInfo("pt-BR"));
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


        private async void BtnConfirmarCartao_Click(
            object sender, RoutedEventArgs e)
        {
            if (pagamentoConfirmado)
                return;

            string nome = TxtNomeCartao.Text.Trim();

            if (string.IsNullOrWhiteSpace(nome))
            {
                MessageBox.Show(
                    "Informe um nome para identificação.",
                    "Validação",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (CarrinhoServiço.Itens.Count == 0)
            {
                MessageBox.Show(
                    "O carrinho está vazio.",
                    "Loja Virtual",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            MessageBoxResult resposta = MessageBox.Show(
                "Deseja confirmar esta compra simulada?\n\n" +
                $"Valor: {ValorCompra:C2}\n\n" +
                "O pedido será registrado como pago " +
                "e o estoque será atualizado.\n\n" +
                "Nenhuma cobrança real será realizada.",
                "Confirmar compra",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resposta != MessageBoxResult.Yes)
                return;

            pagamentoConfirmado = true;
            BtnSalvarCartao.IsEnabled = false;

            try
            {
                PagamentoController controller =
                    new PagamentoController();

                int numeroPedido =
                    await controller.RegistrarCompraCartaoSimuladaAsync(
                        nome,
                        Math.Round(
                            ValorCompra,
                            2,
                            MidpointRounding.AwayFromZero));

                CarrinhoServiço.Limpar();

                MessageBox.Show(
                    $"Pedido Nº {numeroPedido} realizado com sucesso!\n\n" +
                    "Pagamento: Cartão simulado\n" +
                    "Status: Pago\n" +
                    "Estoque atualizado.",
                    "Compra concluída",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                StatusPedido pedidos = new StatusPedido();

                pedidos.Show();
                Close();
            }

            catch (Exception ex)
            {
                pagamentoConfirmado = false;

                MessageBox.Show(
                    "Não foi possível finalizar a compra.\n\n" +
                    ex.Message,
                    "Erro no pagamento",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                BtnSalvarCartao.IsEnabled = !pagamentoConfirmado;
            }
        }


    }
}

