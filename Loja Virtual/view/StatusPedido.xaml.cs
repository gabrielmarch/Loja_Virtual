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
        private List<PedidoStatusExibicao> pedidos = new();

        public StatusPedido()
        {
            InitializeComponent();
            Loaded += StatusPedido_Loaded;
        }

        private async void StatusPedido_Loaded(object sender, RoutedEventArgs e)
        {
            await CarregarPedidosAsync();
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

        private async System.Threading.Tasks.Task CarregarPedidosAsync()
        {
            try
            {
                if (SessaoUsuario.ClienteId == null)
                {
                    MessageBox.Show(
                        "Faça login para visualizar seus pedidos.",
                        "Loja Virtual",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    Close();
                    return;
                }

                int clienteId = SessaoUsuario.ClienteId.Value;

                using var banco = new LojaVirtualDbContext();

                var lista = await banco.pedidos
                    .Where(p => p.ClienteId == clienteId)
                    .Include(p => p.Itens)
                        .ThenInclude(i => i.Produto)
                    .OrderByDescending(p => p.DataPedido)
                    .ToListAsync();

                pedidos = lista.Select(p => new PedidoStatusExibicao
                {
                    IdPedido = p.idPedido,
                    DataPedido = p.DataPedido,
                    Total = p.Subtotal - p.Desconto,
                    EtapaPedido = p.EtapaPedido,
                    Itens = p.Itens.Select(i => new ItemPedidoStatusExibicao
                    {
                        NomeProduto = i.Produto != null ? i.Produto.NomeProduto : "Produto",
                        Descricao = $"Preço unitário: {i.PrecoUnitario:C2}",
                        Quantidade = i.Quantidade,
                        Subtotal = i.Subtotal
                    }).ToList()
                }).ToList();

                ComboPedidos.ItemsSource = pedidos;
                ComboPedidos.DisplayMemberPath = "DescricaoCombo";

                if (pedidos.Count > 0)
                {
                    ComboPedidos.SelectedIndex = 0;
                    TxtSemPedidos.Visibility = Visibility.Collapsed;
                }
                else
                {
                    TxtSemPedidos.Visibility = Visibility.Visible;
                    ListaItensPedido.ItemsSource = null;

                    TxtDataPedido.Text = "-";
                    TxtValorPedido.Text = "R$ 0,00";
                    TxtEtapaAtual.Text = "-";

                    AtualizarEtapasVisuais("Nenhum");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Não foi possível carregar os pedidos.\n\n" + ex.Message,
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ComboPedidos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboPedidos.SelectedItem is not PedidoStatusExibicao pedido)
                return;

            TxtDataPedido.Text = pedido.DataPedido.ToString("dd/MM/yyyy HH:mm");
            TxtValorPedido.Text = pedido.Total.ToString("C2");
            TxtEtapaAtual.Text = FormatarEtapa(pedido.EtapaPedido);

            ListaItensPedido.ItemsSource = pedido.Itens;

            AtualizarEtapasVisuais(pedido.EtapaPedido);
        }


        private string FormatarEtapa(string? etapa)
        {
            return ObterIndiceEtapa(etapa) switch
            {
                1 => "Pedido feito",
                2 => "Produzindo",
                3 => "Enviado",
                4 => "Entregue",
                _ => "Etapa não definida"
            };
        }

        private int ObterIndiceEtapa(string? etapa)
        {
            string valor = (etapa ?? "")
                .Trim()
                .Replace(" ", "")
                .ToUpperInvariant();

            return valor switch
            {
                "PEDIDOFEITO" => 1,
                "PRODUZINDO" => 2,
                "ENVIADO" => 3,
                "ENTREGUE" => 4,
                _ => 0
            };
        }

        private void AtualizarEtapasVisuais(string etapa)
        {
            int indiceAtual = ObterIndiceEtapa(etapa);

            AtualizarEtapa(CirculoEtapa1, Linha1, indiceAtual >= 1, indiceAtual > 1);
            AtualizarEtapa(CirculoEtapa2, Linha2, indiceAtual >= 2, indiceAtual > 2);
            AtualizarEtapa(CirculoEtapa3, Linha3, indiceAtual >= 3, indiceAtual > 3);
            AtualizarUltimaEtapa(CirculoEtapa4, indiceAtual >= 4);
        }

        private void AtualizarEtapa(Border circulo, Border linha, bool ativa, bool concluida)
        {
            if (ativa)
            {
                circulo.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3157A4"));
                circulo.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3157A4"));
            }
            else
            {
                circulo.Background = Brushes.White;
                circulo.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C8D0DD"));
            }

            if (concluida)
            {
                linha.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3157A4"));
            }
            else
            {
                linha.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D9E0EA"));
            }
        }

        private void AtualizarUltimaEtapa(Border circulo, bool ativa)
        {
            if (ativa)
            {
                circulo.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3157A4"));
                circulo.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3157A4"));
            }
            else
            {
                circulo.Background = Brushes.White;
                circulo.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C8D0DD"));
            }
        }
    }

    public class PedidoStatusExibicao
    {
        public int IdPedido { get; set; }

        public DateTime DataPedido { get; set; }

        public decimal Total { get; set; }

        public string EtapaPedido { get; set; } = string.Empty;

        public List<ItemPedidoStatusExibicao> Itens { get; set; } = new();

        public string DescricaoCombo => $"Pedido #{IdPedido}";
    }

    public class ItemPedidoStatusExibicao
    {
        public string NomeProduto { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public int Quantidade { get; set; }

        public decimal Subtotal { get; set; }
    }
}