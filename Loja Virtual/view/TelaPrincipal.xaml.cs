using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Loja_Virtual.controll;
using Loja_Virtual.model;

namespace Loja_Virtual.view
{
    public partial class TelaPrincipal : Window
    {
        private readonly ProdutoController produtoController = new();

        private readonly ObservableCollection<Produto> produtos = new();

        private bool menuVisivel = true;

        public TelaPrincipal()
        {
            InitializeComponent();

            ListaProdutos.ItemsSource = produtos;

            Loaded += TelaPrincipal_Loaded;
        }

        private async void TelaPrincipal_Loaded(
            object sender, RoutedEventArgs e)
        {
            await CarregarProdutosAsync();
        }

        private async Task CarregarProdutosAsync()
        {
            try
            {
                var produtosBanco =
                    await produtoController.ListarProdutosAsync();

                produtos.Clear();

                foreach (var produto in produtosBanco)
                {
                    produtos.Add(produto);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Erro ao carregar os produtos:\n\n" + ex.Message,
                    "Loja Virtual",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
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

        private void button_Carrinho_Click(
            object sender, RoutedEventArgs e)
        {
            CarrinhoCompras carrinho = new CarrinhoCompras();
            carrinho.Show();

            this.Close();
        }

        private void buttonPedidos_Click(
            object sender, RoutedEventArgs e)
        {
            StatusPedido statusPedido = new StatusPedido();
            statusPedido.Show();

            this.Close();
        }

        private void buttonCriarProdutos_Click(
            object sender, RoutedEventArgs e)
        {
            if (!Loja_Virtual.services.SessaoUsuario.EhAdministrador)
            {
                MessageBox.Show(
                    "Apenas administradores podem cadastrar produtos.",
                    "Acesso negado",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            CadastroProduto cadastro = new CadastroProduto();
            cadastro.Show();

            Close();
        }

        private void AdicionarCarrinho_Click(
            object sender, RoutedEventArgs e)
        {
            if (sender is not Button botao ||
                botao.DataContext is not Produto produto)
                return;

            bool adicionado = CarrinhoServiço.Adicionar(produto);

            if (adicionado)
            {
                MessageBox.Show(
                    $"{produto.NomeProduto} foi adicionado ao carrinho!",
                    "Carrinho",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(
                    "Não é possível adicionar mais unidades deste produto.\n" +
                    "Estoque disponível: " + produto.EstoqueProduto,
                    "Estoque insuficiente",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }
        private void Configuracoes_Click(
            object sender, RoutedEventArgs e)
        {
            Configuracoes.Abrir(this, BtnConfiguracoes);
        }

    }
}

