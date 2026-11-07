
using System.Collections.ObjectModel;
using System.Windows;

namespace Loja_Virtual.view
{
    public partial class TelaPrincipal : Window
    {
        private ObservableCollection<ProdutoExibicao> produtos =
            new ObservableCollection<ProdutoExibicao>();

        private bool menuVisivel = true;
        public TelaPrincipal()
        {
            InitializeComponent();

            CarregarProdutos();//
        }


        ///////////////////
        // Código que carrega os produtos na tela princiapal. Apenas para exemplos - Gabriel

        private void CarregarProdutos()
        {
            produtos.Add(new ProdutoExibicao("Mouse", 100.00m));
            produtos.Add(new ProdutoExibicao("Teclado", 150.00m));
            produtos.Add(new ProdutoExibicao("Teclado", 200.00m));
            produtos.Add(new ProdutoExibicao("Mémoria Ram", 600.00m));
            produtos.Add(new ProdutoExibicao("Pc Gamer", 80000.00m));

            produtos.Add(new ProdutoExibicao("Mouse", 125.00m));
            produtos.Add(new ProdutoExibicao("Teclado", 180.00m));
            produtos.Add(new ProdutoExibicao("Notebook", 2900.00m));
            produtos.Add(new ProdutoExibicao("Fone de Ouvido", 35.00m));
            produtos.Add(new ProdutoExibicao("Pc Gamer", 4800.00m));

            produtos.Add(new ProdutoExibicao("Webcam", 225.00m));
            produtos.Add(new ProdutoExibicao("Cadeira Gamer", 320.00m));
            produtos.Add(new ProdutoExibicao("Mesa Gamer", 290.00m));
            produtos.Add(new ProdutoExibicao("Gamer Gamer", 240.00m));
            produtos.Add(new ProdutoExibicao("Fone de Ouvido", 30.00m));

            ListaProdutos.ItemsSource = produtos;
        }

        public class ProdutoExibicao
        {
            public string Nome { get; set; }

            public decimal Preco { get; set; }

            public ProdutoExibicao(string nome, decimal preco)
            {
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
    }
}
///////////////////
