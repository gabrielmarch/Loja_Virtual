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
using System.Threading.Tasks;

namespace Loja_Virtual.view
{
    public partial class PainelAdmin : Window
    {
        private bool menuVisivel = true;

        public PainelAdmin()
        {
            InitializeComponent();
        }
        private async void Window_Loaded(
            object sender, RoutedEventArgs e)
        {
            if (!SessaoUsuario.EhAdministrador ||
                SessaoUsuario.ClienteId == null)
            {
                NegarAcesso();
                return;
            }

            try
            {
                using var banco = new LojaVirtualDbContext();

                int usuarioId = SessaoUsuario.ClienteId.Value;

                bool autorizado = await banco.Clientes
                    .AsNoTracking()
                    .AnyAsync(c =>
                        c.Id == usuarioId &&
                        c.Ativo &&
                        c.PerfilAcesso == "Administrador");

                if (!autorizado)
                {
                    NegarAcesso();
                    return;
                }

                NomeAdministrador.Text =
                    SessaoUsuario.NomeCliente;

                await CarregarIndicadoresAsync(banco);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Não foi possível carregar o painel.\n\n" +
                    ex.Message,
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async Task CarregarIndicadoresAsync(
            LojaVirtualDbContext banco)
        {
            int produtos = await banco.produtos.CountAsync();

            int clientes = await banco.Clientes
                .CountAsync(c => c.PerfilAcesso == "Cliente");

            int pedidos = await banco.pedidos.CountAsync();

            DateTime hoje = DateTime.Today;

            int cupons = await banco.cupoms.CountAsync(c =>
                c.ativo &&
                c.DataValidade >= hoje &&
                c.Desconto > 0m &&
                c.Desconto <= 100m);

            TotalProdutos.Text = produtos.ToString();
            TotalClientes.Text = clientes.ToString();
            TotalPedidos.Text = pedidos.ToString();
            TotalCupons.Text = cupons.ToString();
        }
        private void NegarAcesso()
        {
            MessageBox.Show(
                "Você não possui permissão para acessar " +
                "o Painel Administrativo.",
                "Acesso negado",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            SessaoUsuario.Encerrar();
            CarrinhoServiço.Limpar();

            Login login = new Login();
            login.Show();

            Close();
        }

        private void GerenciarCupons_Click(
            object sender, RoutedEventArgs e)
        {
            if (!SessaoUsuario.EhAdministrador)
            {
                NegarAcesso();
                return;
            }

            GerenciarCupons tela = new GerenciarCupons();

            tela.Show();
            Close();
        }

        private void GerenciarProdutos_Click(
            object sender, RoutedEventArgs e)
        {
            if (!SessaoUsuario.EhAdministrador)
            {
                NegarAcesso();
                return;
            }

            CadastroProduto cadastro = new CadastroProduto();
            cadastro.Show();

            Close();
        }

        private void VisualizarLoja_Click(
            object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "A navegação entre a loja e o painel " +
                "será configurada posteriormente.",
                "Visualizar Loja",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void buttonMenu_Click(
            object sender, RoutedEventArgs e)
        {
            menuVisivel = !menuVisivel;

            if (menuVisivel)
            {
                BarraLateral.Visibility = Visibility.Visible;
                ColunaMenu.Width = new GridLength(215);
            }
            else
            {
                BarraLateral.Visibility = Visibility.Collapsed;
                ColunaMenu.Width = new GridLength(0);
            }
        }

        private void Sair_Click(
            object sender, RoutedEventArgs e)
        {
            MessageBoxResult resposta = MessageBox.Show(
                "Deseja realmente sair da conta administrativa?",
                "Confirmar saída",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resposta != MessageBoxResult.Yes)
                return;

            CarrinhoServiço.Limpar();
            SessaoUsuario.Encerrar();

            Login login = new Login();
            login.Show();

            Close();
        }

        private void GerenciarPedidos_Click(
            object sender, RoutedEventArgs e)
        {
            if (!SessaoUsuario.EhAdministrador)
            {
                NegarAcesso();
                return;
            }

            GerenciarPedidos tela = new GerenciarPedidos();

            tela.Show();
            Close();
        }


    }
}
