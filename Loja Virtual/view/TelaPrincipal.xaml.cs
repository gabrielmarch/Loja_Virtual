
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
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

        // CARREGAR PRODUTOS DO BANCO DE DADOS
        private async Task CarregarProdutosAsync()
        {
            try
            {
                // Busca os produtos no banco
                var produtosBanco =
                    await produtoController.ListarProdutosAsync();

                // Limpa a lista atual
                produtos.Clear();

                // Adiciona os produtos encontrados
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
            CadastroProduto cadastroProdutos = new CadastroProduto();
            cadastroProdutos.Show();

            this.Close();
        }
    }
}

