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
using Loja_Virtual.services;
using System.Threading.Tasks;

namespace Loja_Virtual.view
{
    public partial class GerenciarPedidos : Window
    {
        private readonly GerenciamentoPedidoController _controller =
            new GerenciamentoPedidoController();

        private bool _processando = false;

        public GerenciarPedidos()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(
            object sender, RoutedEventArgs e)
        {
            if (!SessaoUsuario.EhAdministrador)
            {
                MessageBox.Show(
                    "Acesso permitido apenas para administradores.",
                    "Acesso negado",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                SessaoUsuario.Encerrar();

                Login login = new Login();
                login.Show();

                Close();
                return;
            }

            await CarregarPedidosAsync();
        }

        private async Task CarregarPedidosAsync(
            int? selecionarPedidoId = null)
        {
            try
            {
                var pedidos = await _controller.ListarPedidosAsync();

                ListaPedidos.ItemsSource = pedidos;

                ListaPedidos.SelectedItem = null;

                if (selecionarPedidoId.HasValue)
                {
                    foreach (var pedido in pedidos)
                    {
                        if (pedido.IdPedido == selecionarPedidoId.Value)
                        {
                            ListaPedidos.SelectedItem = pedido;
                            break;
                        }
                    }
                }

                if (ListaPedidos.SelectedItem == null)
                    LimparDetalhes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Erro ao carregar pedidos:\n\n" + ex.Message,
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ListaPedidos_SelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            if (ListaPedidos.SelectedItem
                is not PedidoAdminExibicao pedido)
            {
                LimparDetalhes();
                return;
            }

            TxtPedidoSelecionado.Text =
                $"Pedido Nº {pedido.IdPedido} - {pedido.NomeCliente}";

            TxtPagamento.Text =
                $"Pagamento: {pedido.StatusPagamento}";

            TxtEtapaAtual.Text =
                $"Etapa atual: {pedido.EtapaFormatada}";

            string? proxima = ObterProximaEtapa(pedido.EtapaPedido);

            if (pedido.StatusPagamento != "Pago")
            {
                TxtProximaEtapa.Text =
                    "Aguardando confirmação do pagamento.";

                BtnAvancar.IsEnabled = false;
            }
            else if (proxima == null)
            {
                TxtProximaEtapa.Text =
                    "Pedido entregue ou etapa não reconhecida.";

                BtnAvancar.IsEnabled = false;
            }
            else
            {
                TxtProximaEtapa.Text =
                    $"Próxima etapa: {proxima}";

                BtnAvancar.IsEnabled = !_processando;
            }
        }

        private static string? ObterProximaEtapa(string atual)
        {
            return atual switch
            {
                "PedidoFeito" => "Produzindo",
                "Produzindo" => "Enviado",
                "Enviado" => "Entregue",
                _ => null
            };
        }

        private async void AvancarEtapa_Click(
            object sender, RoutedEventArgs e)
        {
            if (_processando ||
                ListaPedidos.SelectedItem
                    is not PedidoAdminExibicao pedido)
            {
                return;
            }

            string? proxima =
                ObterProximaEtapa(pedido.EtapaPedido);

            if (proxima == null ||
                pedido.StatusPagamento != "Pago")
            {
                MessageBox.Show(
                    "Este pedido não pode avançar.",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult resposta = MessageBox.Show(
                $"Deseja atualizar o pedido Nº {pedido.IdPedido}?\n\n" +
                $"Etapa atual: {pedido.EtapaFormatada}\n" +
                $"Próxima etapa: {proxima}",
                "Confirmar alteração",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resposta != MessageBoxResult.Yes)
                return;

            int pedidoId = pedido.IdPedido;

            _processando = true;
            BtnAvancar.IsEnabled = false;

            try
            {
                string novaEtapa =
                    await _controller.AvancarEtapaAsync(pedidoId);

                MessageBox.Show(
                    $"Pedido Nº {pedidoId} atualizado!\n\n" +
                    $"Nova etapa: {novaEtapa}",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                await CarregarPedidosAsync(pedidoId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Não foi possível atualizar o pedido.\n\n" +
                    ex.Message,
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                await CarregarPedidosAsync(pedidoId);
            }
            finally
            {
                _processando = false;

                ListaPedidos_SelectionChanged(
                    ListaPedidos,
                    new SelectionChangedEventArgs(
                        DataGrid.SelectionChangedEvent,
                        Array.Empty<object>(),
                        Array.Empty<object>()));
            }
        }

        private async void AtualizarLista_Click(
            object sender, RoutedEventArgs e)
        {
            if (_processando)
                return;

            int? id = (ListaPedidos.SelectedItem
                as PedidoAdminExibicao)?.IdPedido;

            await CarregarPedidosAsync(id);
        }

        private void LimparDetalhes()
        {
            TxtPedidoSelecionado.Text =
                "Selecione um pedido na tabela.";

            TxtPagamento.Text = "Pagamento: -";
            TxtEtapaAtual.Text = "Etapa atual: -";
            TxtProximaEtapa.Text = "Próxima etapa: -";
            BtnAvancar.IsEnabled = false;
        }

        private void VoltarPainel_Click(
            object sender, RoutedEventArgs e)
        {
            if (_processando)
                return;

            PainelAdmin painel = new PainelAdmin();
            painel.Show();

            Close();
        }
    }
}
