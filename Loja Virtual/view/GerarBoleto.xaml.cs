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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Loja_Virtual.view
{
    public partial class GerarBoleto : Window
    {
        private bool menuVisivel = true;


        public decimal ValorCompra { get; private set; }

        public GerarBoleto() : this(0m)
        {
        }

        public GerarBoleto(decimal valorCompra)
        {
            InitializeComponent();

            ValorCompra = valorCompra;
            TxtValor.Text = valorCompra.ToString(
                "C2",
                System.Globalization.CultureInfo.GetCultureInfo("pt-BR"));
            TxtVencimento.Text = DateTime.Today
                .AddDays(3)
                .ToString("dd/MM/yyyy");
            TxtCodigoBarras.Text =
                "Simulação acadêmica — boleto não emitido.";
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

