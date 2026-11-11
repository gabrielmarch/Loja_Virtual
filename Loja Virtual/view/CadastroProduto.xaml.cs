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
using System.Globalization;
using Loja_Virtual.controll;
using Loja_Virtual.model;

namespace Loja_Virtual.view
{
    public partial class CadastroProduto : Window
    {
        private bool menuVisivel = true;

        private readonly ProdutoController produtoController = new();

        public CadastroProduto()
        {
            InitializeComponent();

            Loaded += CadastroProduto_Loaded;
        }

        // CARREGAR FORNECEDORES
        private async void CadastroProduto_Loaded(
            object sender, RoutedEventArgs e)
        {
            try
            {
                var fornecedores =
                    await produtoController.ListarFornecedoresAsync();

                cmbFornecedor.ItemsSource = fornecedores;

                if (fornecedores.Count == 0)
                {
                    MessageBox.Show(
                        "Nenhum fornecedor foi encontrado.\n" +
                        "Cadastre um fornecedor antes de cadastrar produtos.",
                        "Loja Virtual",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Erro ao carregar fornecedores:\n" + ex.Message,
                    "Loja Virtual",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // CONFIRMAR CADASTRO
        private async void buttonConfirmarCad_Click(
            object sender, RoutedEventArgs e)
        {
            // Validar nome
            if (string.IsNullOrWhiteSpace(txtNomeProduto.Text))
            {
                MessageBox.Show("Informe o nome do produto.");
                txtNomeProduto.Focus();
                return;
            }

            // Validar preço
            if (!decimal.TryParse(
                txtPrecoProduto.Text,
                NumberStyles.Currency,
                CultureInfo.GetCultureInfo("pt-BR"),
                out decimal preco) || preco <= 0)
            {
                MessageBox.Show("Informe um preço válido maior que zero.");
                txtPrecoProduto.Focus();
                return;
            }

            // Validar estoque
            if (!int.TryParse(txtEstoqueProduto.Text, out int estoque)
                || estoque < 0)
            {
                MessageBox.Show("Informe um estoque válido.");
                txtEstoqueProduto.Focus();
                return;
            }

            // Validar fornecedor
            if (cmbFornecedor.SelectedValue is not int fornecedorId)
            {
                MessageBox.Show("Selecione um fornecedor.");
                cmbFornecedor.Focus();
                return;
            }

            // Criar o produto com os dados do formulário
            Produto produto = new Produto
            {
                NomeProduto = txtNomeProduto.Text.Trim(),
                Preco = preco,
                Unidade = txtUnidadeProduto.Text.Trim(),
                EstoqueProduto = estoque,
                CodigoBarras = txtCodBarrasProd.Text.Trim(),
                DescricaoProduto = txtDescri.Text.Trim(),
                FornecedorId = fornecedorId,
                Peso = 0m
            };

            try
            {
                // Evita dois cadastros por cliques repetidos
                buttonConfirmarCad.IsEnabled = false;

                // Salvar no SQL Server
                int idGerado =
                    await produtoController.CadastrarProdutoAsync(produto);

                // Exibir o ID gerado
                txtIdProduto.Text = idGerado.ToString();

                MessageBox.Show(
                    $"Produto cadastrado com sucesso!\nID: {idGerado}",
                    "Loja Virtual",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                // Abrir o catálogo atualizado
                TelaPrincipal principal = new TelaPrincipal();
                principal.Show();

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
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Não foi possível cadastrar o produto:\n" +
                    ex.Message,
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                buttonConfirmarCad.IsEnabled = true;
            }
        }

        // ABRIR E FECHAR MENU LATERAL
        private void buttonMenu_Click(
            object sender, RoutedEventArgs e)
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
