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
    public partial class Login : Window
    {
        private readonly LoginController _controller;

        public Login()
        {
            InitializeComponent();

            _controller = new LoginController();
        }


        private void BtnEntrar_Click(object sender, RoutedEventArgs e)
        {

            Cadastro cadastro = new Cadastro();

            cadastro.Show();

            this.Close();

            bool autenticado = _controller.Autenticar(
                txtCpfCnpj.Text,
                txtSenha.Password,
                out string mensagem
            );

            if (!autenticado)
            {
                MessageBox.Show(
                    mensagem,
                    "Login",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );

                return;
            }

            MessageBox.Show(
                mensagem,
                "Login",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );

            TelaPrincipal principal = new TelaPrincipal();

            principal.Show();

            this.Close();
        }


        private void BtnCriarConta_Click(object sender, RoutedEventArgs e)
        {
            Cadastro cadastro = new Cadastro();

            cadastro.Show();

            this.Close();
        }
    }
}
