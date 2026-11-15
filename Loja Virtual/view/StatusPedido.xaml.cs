using Loja_Virtual.data;
using Loja_Virtual.model;
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
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Loja_Virtual.data;


namespace Loja_Virtual.view
{
    public partial class StatusPedido : Window
    {
        private bool menuVisivel = true;

        public StatusPedido()
        {
            InitializeComponent();

            CarregarPedidos();
        }

        private void CarregarPedidos()
        {
            try
            {
                int clienteId = SessaoUsuario.ClienteId;

                using (var contexto = new LojaVirtualDbContext())
                {
                    var pedidos = contexto.Pedidos
                        .AsNoTracking()
                        .Where(p => p.ClienteId == clienteId)
                        .Include(p => p.Itens)
                            .ThenInclude(i => i.Produto)
                        .OrderByDescending(p => p.DataPedido)
                        .ToList();

                    var pedidosExibicao = pedidos.Select(p => new
                    {
                        NumeroPedido = "Pedido #" + p.Id,
                        DataPedido = p.DataPedido,
                        Status = p.Status,
                        ValorTotal = p.ValorTotal,

                        Itens = p.Itens.Select(i => new
                        {
                            NomeProduto = i.Produto.NomeProduto,
                            Quantidade = i.Quantidade,
                            Subtotal = i.Subtotal
                        }).ToList()

                    }).ToList();

                    ListaPedidos.ItemsSource = pedidosExibicao;

                    MensagemSemPedidos.Visibility =
                        pedidosExibicao.Count == 0
                        ? Visibility.Visible
                        : Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Erro ao carregar os pedidos: " + ex.Message,
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // ABRIR E FECHAR O MENU HAMBÚRGUER
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
