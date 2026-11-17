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
using Loja_Virtual.data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Loja_Virtual.view
{
    public partial class Cupons : Window
    {
        private bool menuVisivel = true;

        public Cupons()
        {
            InitializeComponent();

            Loaded += Cupons_Loaded;
        }

        private async void Cupons_Loaded(
            object sender, RoutedEventArgs e)
        {
            await CarregarCuponsAsync();
        }

        private async Task CarregarCuponsAsync()
        {
            try
            {
                using var banco = new LojaVirtualDbContext();

                DateTime hoje = DateTime.Today;

                var cupons = await banco.cupoms
                    .AsNoTracking()
                    .Where(c =>
                        c.ativo &&
                        c.DataValidade >= hoje &&
                        c.Desconto > 0 &&
                        c.Desconto <= 100)
                    .OrderBy(c => c.Desconto)
                    .ToListAsync();

                ListaCupons.ItemsSource = cupons;

                MensagemSemCupons.Visibility =
                    cupons.Count == 0
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Não foi possível carregar os cupons.\n\n" +
                    ex.Message,
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void CopiarCupom_Click(
            object sender, RoutedEventArgs e)
        {
            if (sender is not Button botao)
                return;

            string codigo = botao.Tag?.ToString() ?? "";

            if (string.IsNullOrWhiteSpace(codigo))
                return;

            Clipboard.SetText(codigo);

            MessageBox.Show(
                $"Cupom {codigo} copiado!\n\n" +
                "Cole o código no campo de cupom do carrinho.",
                "Cupom copiado",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        private void IrCarrinho_Click(
            object sender, RoutedEventArgs e)
        {
            CarrinhoCompras carrinho = new CarrinhoCompras();

            carrinho.Show();
            Close();
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
