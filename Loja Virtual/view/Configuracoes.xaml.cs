using Loja_Virtual.controll;
using Loja_Virtual.services;
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
using System.Windows.Controls;
using System.Windows.Media;

namespace Loja_Virtual.view
{
    public partial class Configuracoes : Window
    {
        private string? destino;

        public Configuracoes()
        {
            InitializeComponent();
        }
        public static void Abrir(Window janelaAtual, Button botao)
        {
            Configuracoes configuracoes = new Configuracoes
            {
                Owner = janelaAtual
            };
            Point posicao = botao.PointToScreen(
                new Point(0, botao.ActualHeight));

            configuracoes.Left =
                posicao.X + botao.ActualWidth - configuracoes.Width;

            configuracoes.Top = posicao.Y + 5;
            var area = SystemParameters.WorkArea;

            configuracoes.Left = Math.Max(
                area.Left,
                Math.Min(configuracoes.Left,
                         area.Right - configuracoes.Width));

            configuracoes.Top = Math.Max(
                area.Top,
                Math.Min(configuracoes.Top,
                         area.Bottom - configuracoes.Height));

            configuracoes.ShowDialog();

            if (configuracoes.destino == "Perfil")
            {
                Perfil perfil = new Perfil();
                perfil.Show();
                janelaAtual.Close();
            }
            else if (configuracoes.destino == "Login")
            {
                CarrinhoServiço.Limpar();
                SessaoUsuario.Encerrar();

                Login login = new Login();
                login.Show();
                janelaAtual.Close();
            }
        }


        private void MinhaConta_Click(
            object sender, RoutedEventArgs e)
        {
            destino = "Perfil";
            Close();
        }

        private void SairConta_Click(
            object sender, RoutedEventArgs e)
        {
            MessageBoxResult resposta = MessageBox.Show(
                "Deseja realmente sair da sua conta?",
                "Confirmar saída",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resposta != MessageBoxResult.Yes)
                return;

            destino = "Login";
            Close();
        }
    }
}
