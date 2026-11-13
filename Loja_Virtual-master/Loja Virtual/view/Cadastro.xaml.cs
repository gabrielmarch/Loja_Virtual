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

            MessageBox.Show(mensagem);

            if (valido)
            {
                // Posteriormente salvaremos o cliente no banco.
            }
        }

        private void BtnEntrar_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();

            login.Show();

            this.Close();
        }
    }
}
