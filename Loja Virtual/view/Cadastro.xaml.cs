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
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace Loja_Virtual.view
{
    public partial class Cadastro : Window
    {
        private readonly CadastroController _controller = new();

        public Cadastro()
        {
            InitializeComponent();
        }

        
        private void txtCpfCnpj_TextChanged(
            object sender, TextChangedEventArgs e)
        {
            string documento = new string(
                txtCpfCnpj.Text.Where(char.IsDigit).ToArray());

            CamposPessoaFisica.Visibility =
                documento.Length == 11
                ? Visibility.Visible
                : Visibility.Collapsed;

            CamposPessoaJuridica.Visibility =
                documento.Length == 14
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        
        private async void BtnCadastrar_Click(
            object sender, RoutedEventArgs e)
        {
            try
            {
                BtnCadastrar.IsEnabled = false;

                int idCliente =
                    await _controller.CadastrarClienteAsync(
                        txtNome.Text,
                        txtEmail.Text,
                        txtCpfCnpj.Text,
                        txtTelefone.Text,
                        txtEndereco.Text,
                        txtSenha.Password,
                        txtConfirmarSenha.Password,
                        dpNascimento.SelectedDate,
                        txtRazaoSocial.Text,
                        txtNomeFantasia.Text);

                MessageBox.Show(
                    $"Cliente cadastrado com sucesso!\nID: {idCliente}",
                    "Loja Virtual",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                Login login = new Login();
                login.Show();
                this.Close();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Validação",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            catch (DbUpdateException)
            {
                MessageBox.Show(
                    "Não foi possível salvar o cliente. " +
                    "Confira se o CPF/CNPJ ou e-mail já está cadastrado.",
                    "Erro de cadastro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Erro ao cadastrar cliente:\n" + ex.Message,
                    "Loja Virtual",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                BtnCadastrar.IsEnabled = true;
            }
        }

        private void BtnEntrar_Click(
            object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
    }
}
