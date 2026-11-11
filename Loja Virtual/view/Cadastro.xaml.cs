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

    public partial class Cadastro : Window
    {
        private readonly CadastroController _controller;

        public Cadastro()
        {
            InitializeComponent();

            _controller = new CadastroController();
        }

        private void BtnEntrar_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void BtnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            bool valido = _controller.ValidarCadastro(
                txtNome.Text,
                txtEmail.Text,
                txtCpfCnpj.Text,
                txtSenha.Password,
                txtConfirmarSenha.Password,
                out string mensagem
            );

            if (!valido)
            {
                MessageBox.Show(mensagem);
                return;
            }

            bool cadastrado = _controller.CadastrarCliente(
                txtNome.Text,
                txtEmail.Text,
                txtCpfCnpj.Text,
                txtSenha.Password,
                out mensagem
            );

            MessageBox.Show(mensagem);

            if (cadastrado)
            {
                txtNome.Clear();
                txtEmail.Clear();
                txtCpfCnpj.Clear();
                txtSenha.Clear();
                txtConfirmarSenha.Clear();
            }
        }


        private void txtNome_TextChanged(object sender, TextChangedEventArgs e)
        {
            phNome.Visibility = string.IsNullOrEmpty(txtNome.Text)
                ? Visibility.Visible : Visibility.Collapsed;
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            phEmail.Visibility = string.IsNullOrEmpty(txtEmail.Text)
                ? Visibility.Visible : Visibility.Collapsed;
        }

        private void txtCpfCnpj_TextChanged(object sender, TextChangedEventArgs e)
        {
            phCpfCnpj.Visibility = string.IsNullOrEmpty(txtCpfCnpj.Text)
                ? Visibility.Visible : Visibility.Collapsed;
        }

        private void txtSenha_PasswordChanged(object sender, RoutedEventArgs e)
        {
            phSenha.Visibility = string.IsNullOrEmpty(txtSenha.Password)
                ? Visibility.Visible : Visibility.Collapsed;
        }

        private void txtConfirmarSenha_PasswordChanged(object sender, RoutedEventArgs e)
        {
            phConfirmarSenha.Visibility = string.IsNullOrEmpty(txtConfirmarSenha.Password)
                ? Visibility.Visible : Visibility.Collapsed;
        }

    }
}
