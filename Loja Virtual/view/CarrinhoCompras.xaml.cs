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

namespace Loja_Virtual.view
{
    /// <summary>
    /// Lógica interna para CarrinhoCompras.xaml
    /// </summary>
    public partial class CarrinhoCompras : Window
    {
        public CarrinhoCompras()
        {
            InitializeComponent();
        }


        private void RemoverProduto1_Click(object sender, RoutedEventArgs e)
        {
            ///dfdf
        }

        private void RemoverProduto2_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Quantidade_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void AplicarCupom_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Pagamento_Click(object sender, RoutedEventArgs e)
        {
        }

        private void button_VoltarCarHome_Click(object sender, RoutedEventArgs e)
        {
            TelaPrincipal telaPrincipal = new TelaPrincipal();
            telaPrincipal.Show();

            this.Close();
        }
    }
}
