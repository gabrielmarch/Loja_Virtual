using Loja_Virtual.data;
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
    public partial class Perfil : Window
    {
        private readonly int clienteId;

        private bool menuVisivel = true;

        public Perfil(int idCliente)
        {
            InitializeComponent();

            clienteId = idCliente;

            CarregarPerfil();
        }

        private void CarregarPerfil()
        {
            using (var contexto = new LojaVirtualDbContext())
            {
                var cliente = contexto.Clientes
                    .FirstOrDefault(c => c.Id == clienteId);

                if (cliente == null)
                {
                    MessageBox.Show("Cliente não encontrado.");
                    return;
                }

                NomeUsuario.Text = cliente.Nome;
                EmailUsuario.Text = cliente.Email;

                int quantidade = contexto.Pedidos
                    .Count(p => p.ClienteId == clienteId);

                QuantidadePedidos.Text = quantidade.ToString();
            }
        }

        // ABRIR E FECHAR MENU LATERAL
        private void buttonMenu_Click(
            object sender,
            RoutedEventArgs e)
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
