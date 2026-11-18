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
        private void Voltar_Click(object sender, RoutedEventArgs e)
        {
            SuporteCliente suporte = new SuporteCliente();
            suporte.Show();
            this.Close();
        }

        private void BtnEnviarMensagem_Click(
            object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNome.Text) ||
                string.IsNullOrWhiteSpace(TxtEmail.Text) ||
                string.IsNullOrWhiteSpace(TxtMensagem.Text))
            {
                MessageBox.Show(
                    "Preencha todos os campos antes de enviar.",
                    "Campos obrigatórios",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            MessageBox.Show(
                "Mensagem enviada",
                "Ouvidoria",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            TxtNome.Clear();
            TxtEmail.Clear();
            TxtMensagem.Clear();
        }


    }
}

