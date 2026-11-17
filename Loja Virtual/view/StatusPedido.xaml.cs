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
using Loja_Virtual.data;
using Loja_Virtual.services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;


namespace Loja_Virtual.view
{
    public partial class StatusPedido : Window
    {
        private bool menuVisivel = true;
        private bool processandoPagamento = false;

        public StatusPedido()
        {
            InitializeComponent();

            Loaded += StatusPedido_Loaded;
        }

        public class PedidoExibicao
        {
            public int Id { get; set; }

            public DateTime DataPedido { get; set; }

            public decimal Total { get; set; }

            public string Status { get; set; }
                = string.Empty;
        }

        private async void StatusPedido_Loaded(
            object sender, RoutedEventArgs e)
        {
            if (!SessaoUsuario.EstaAutenticado)
            {
                MessageBox.Show(
                    "Faça login para visualizar seus pedidos.",
                    "Acesso restrito",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                Login login = new Login();
                login.Show();
                Close();
                return;
            }

            await CarregarPedidosAsync();
        }

        private async Task CarregarPedidosAsync()
        {
            try
            {
                int clienteId =
                    SessaoUsuario.ClienteId!.Value;

                using var banco = new LojaVirtualDbContext();

                var pedidos = await banco.pedidos
                    .AsNoTracking()
                    .Where(p => p.ClienteId == clienteId)
                    .OrderByDescending(p => p.DataPedido)
                    .Select(p => new PedidoExibicao
                    {
                        Id = p.idPedido,
                        DataPedido = p.DataPedido,
                        Total = p.Subtotal - p.Desconto,
                        Status = p.Status
                    })
                    .ToListAsync();

                ListaPedidos.ItemsSource = pedidos;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Erro ao carregar os pedidos:\n" +
                    ex.Message,
                    "Loja Virtual",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async void AtualizarPedidos_Click(
            object sender, RoutedEventArgs e)
        {
            await CarregarPedidosAsync();
        }

        private async void ConfirmarPagamento_Click(
            object sender, RoutedEventArgs e)
        {
            if (processandoPagamento)
                return;

            if (ListaPedidos.SelectedItem
                is not PedidoExibicao pedido)
            {
                MessageBox.Show(
                    "Selecione um pedido primeiro.",
                    "Meus Pedidos",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (pedido.Status != "AguardandoPagamento")
            {
                MessageBox.Show(
                    "Somente pedidos aguardando pagamento " +
                    "podem ser confirmados.",
                    "Meus Pedidos",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult resposta = MessageBox.Show(
                $"Simular o pagamento do pedido Nº {pedido.Id}?\n\n" +
                $"Valor: {pedido.Total:C2}\n\n" +
                "Esta operação é apenas acadêmica. " +
                "Nenhuma cobrança bancária será realizada.",
                "Confirmar simulação",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resposta != MessageBoxResult.Yes)
                return;

            processandoPagamento = true;
            BtnConfirmarPagamento.IsEnabled = false;

            try
            {
                PagamentoController controller =
                    new PagamentoController();

                await controller
                    .ConfirmarBoletoSimuladoAsync(pedido.Id);

                MessageBox.Show(
                    $"Pagamento simulado do pedido " +
                    $"Nº {pedido.Id} confirmado!\n\n" +
                    "Status: Pago.\n" +
                    "Estoque atualizado com sucesso.",
                    "Loja Virtual",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                await CarregarPedidosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Não foi possível confirmar o pagamento.\n\n" +
                    ex.Message,
                    "Erro no pagamento",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                processandoPagamento = false;
                BtnConfirmarPagamento.IsEnabled = true;
            }
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
