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
    public partial class GerarBoleto : Window
    {
        private bool menuVisivel = true;
        private bool pedidoConfirmado = false;

        public decimal ValorCompra { get; private set; }

        public GerarBoleto() : this(0m)
        {
        }

        public GerarBoleto(decimal valorCompra)
        {
            InitializeComponent();

            ValorCompra = valorCompra;
            TxtValor.Text = valorCompra.ToString(
                "C2",
                System.Globalization.CultureInfo.GetCultureInfo("pt-BR"));
            TxtVencimento.Text = DateTime.Today
                .AddDays(3)
                .ToString("dd/MM/yyyy");
            TxtCodigoBarras.Text =
                "Simulação acadêmica — boleto não emitido.";
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


        private async void BtnConfirmarPedido_Click(
            object sender, RoutedEventArgs e)
        {
            if (pedidoConfirmado)
                return;

            MessageBoxResult resposta = MessageBox.Show(
                "Deseja confirmar o pedido com pagamento por boleto?\n\n" +
                "O pedido ficará aguardando pagamento.",
                "Confirmar pedido",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resposta != MessageBoxResult.Yes)
                return;

            pedidoConfirmado = true;
            BtnConfirmarPedido.IsEnabled = false;

            try
            {
                PedidoController controller = new PedidoController();

                int numeroPedido =
                    await controller.RegistrarPedidoAsync();

                CarrinhoServiço.Limpar();

                MessageBox.Show(
                    $"Pedido Nº {numeroPedido} registrado com sucesso!\n\n" +
                    "Status: Aguardando pagamento.\n\n" +
                    "Este boleto é apenas uma simulação acadêmica.",
                    "Pedido registrado",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                TelaPrincipal principal = new TelaPrincipal();
                principal.Show();

                Close();
            }
            catch (Exception ex)
            {
                pedidoConfirmado = false;
                BtnConfirmarPedido.IsEnabled = true;

                MessageBox.Show(
                    "Não foi possível registrar o pedido.\n\n" +
                    ex.Message,
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


    }
}

