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
    public partial class Ouvidoria : Window
    {
        private bool menuVisivel = true;

        public Ouvidoria()
        {
            InitializeComponent();
        }

        // ABRIR E FECHAR MENU HAMBÚRGUER
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

        // VOLTAR PARA A TELA DE SUPORTE
        private void Voltar_Click(object sender, RoutedEventArgs e)
        {
            SuporteCliente suporte = new SuporteCliente();
            suporte.Show();
            this.Close();
        }
    }
}

