using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Loja_Virtual.data;
using Loja_Virtual.model;
using Loja_Virtual.services;

namespace Loja_Virtual.view
{
    public partial class TelaPrincipal : Window
    {
        private ObservableCollection<ProdutoExibicao> produtos =
            new ObservableCollection<ProdutoExibicao>();

        private bool menuVisivel = true;

        private readonly int clienteId;

        public TelaPrincipal(int idCliente)
        {
            InitializeComponent();

            clienteId = idCliente;

            CarregarProdutos();
        }

        private void CarregarProdutos()
        {
            using (var contexto = new LojaVirtualDbContext())
            {
                var lista = contexto.Produtos
                    .Select(p => new ProdutoExibicao(
                        p.Id,
                        p.NomeProduto,
                        p.Preco
                    ))
                    .ToList();

                produtos.Clear();

                foreach (var produto in lista)
                {
                    produtos.Add(produto);
                }
            }

            ListaProdutos.ItemsSource = produtos;
        }

        private void Produto_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement elemento &&
                elemento.DataContext is ProdutoExibicao produto)
            {
                CarrinhoService.Adicionar(
                    produto.Id,
                    produto.Nome,
                    produto.Preco
                );

                MessageBox.Show(
                    $"{produto.Nome} foi adicionado ao carrinho!",
                    "Carrinho",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
        }

        public class ProdutoExibicao
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public decimal Preco { get; set; }

            public ProdutoExibicao(int id, string nome, decimal preco)
            {
                Id = id;
                Nome = nome;
                Preco = preco;
            }
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

        private void button_Carrinho_Click(object sender, RoutedEventArgs e)
        {
            CarrinhoCompras carrinho = new CarrinhoCompras();
            carrinho.Show();
            this.Close();
        }

        private void buttonPedidos_Click(object sender, RoutedEventArgs e)
        {
            StatusPedido statusPedido = new StatusPedido();
            statusPedido.Show();
            this.Close();
        }

        private void buttonCriarProdutos_Click(object sender, RoutedEventArgs e)
        {
            CadastroProduto cadastroProdutos = new CadastroProduto();
            cadastroProdutos.Show();
            this.Close();
        }
    }
}
