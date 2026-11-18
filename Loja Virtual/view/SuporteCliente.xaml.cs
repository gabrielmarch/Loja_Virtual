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
    public partial class SuporteCliente : Window
    {
        private bool menuVisivel = true;

        public SuporteCliente()
        {
            InitializeComponent();
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

        private void button_IniciarChat_Click(
            object sender, RoutedEventArgs e)
        {
            ChatSuporte chat = new ChatSuporte();
            chat.Show();
            this.Close();
        }

        private void button_AcessarOuvidoria_Click(
            object sender, RoutedEventArgs e)
        {
            Ouvidoria ouvidoria = new Ouvidoria();
            ouvidoria.Show();
            this.Close();
        }
    }
}

