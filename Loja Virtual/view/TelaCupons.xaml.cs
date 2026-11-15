using Loja_Virtual.controll;
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
    public partial class TelaCupons : Window
    {
        private readonly CuponsController _controller;

        public TelaCupons()
        {
            InitializeComponent();

            _controller = new CuponsController();

            CarregarCupons();
        }

        private void CarregarCupons()
        {
            try
            {
                ListaCupons.ItemsSource =
                    _controller.ListarCupons();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Erro ao carregar os cupons: " + ex.Message,
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void buttonMenu_Click(
            object sender,
            RoutedEventArgs e)
        {
            // Comportamento do menu.
        }
    }
}