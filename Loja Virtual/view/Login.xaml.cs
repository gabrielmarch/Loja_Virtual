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
using Loja_Virtual.services;

namespace Loja_Virtual.view
{
    public partial class Login : Window
    {
        private readonly LoginController _controller = new();

        public Login()
        {
            InitializeComponent();
        }

        
        private async void BtnEntrar_Click(
            object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCpfCnpj.Text) ||
                    string.IsNullOrWhiteSpace(txtSenha.Password))
                {
                    MessageBox.Show(
                        "Informe CPF/CNPJ e senha.",
                        "Validação",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                
                if (sender is System.Windows.Controls.Button botao)
                    botao.IsEnabled = false;

                var cliente = await _controller.AutenticarAsync(
                    txtCpfCnpj.Text,
                    txtSenha.Password);

                if (cliente == null)
                {
                    MessageBox.Show(
                        "CPF/CNPJ ou senha incorretos.",
                        "Falha no login",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                
                SessaoUsuario.Iniciar(cliente);

                TelaPrincipal principal = new TelaPrincipal();
                principal.Show();

                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Não foi possível realizar o login. " +
                    "Verifique a conexão com o banco de dados.",
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                if (sender is System.Windows.Controls.Button botao)
                    botao.IsEnabled = true;
            }
        }

        
        private void BtnCriarConta_Click(
            object sender, RoutedEventArgs e)
        {
            Cadastro cadastro = new Cadastro();
            cadastro.Show();

            this.Close();
        }
    }
}

