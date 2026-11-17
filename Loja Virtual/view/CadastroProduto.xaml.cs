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
using Microsoft.Win32;
using System.IO;
using System.Globalization;
using Loja_Virtual.controll;
using Loja_Virtual.model;
using Loja_Virtual.services;

namespace Loja_Virtual.view
{
    public partial class CadastroProduto : Window
    {
        private string? caminhoImagemSelecionada = null;

        private readonly ProdutoController produtoController = new();

        public CadastroProduto()
        {
            InitializeComponent();

            Loaded += CadastroProduto_Loaded;
        }

        private async void CadastroProduto_Loaded(
            object sender, RoutedEventArgs e)
        {
            if (!SessaoUsuario.EhAdministrador)
            {
                MessageBox.Show(
                    "Apenas administradores podem acessar " +
                    "o cadastro de produtos.",
                    "Acesso negado",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                if (SessaoUsuario.EstaAutenticado)
                {
                    TelaPrincipal principal = new TelaPrincipal();
                    principal.Show();
                }
                else
                {
                    Login login = new Login();
                    login.Show();
                }

                Close();
                return;
            }

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

        private async void buttonConfirmarCad_Click(
            object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNomeProduto.Text))
            {
                MessageBox.Show("Informe o nome do produto.");
                txtNomeProduto.Focus();
                return;
            }

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

            if (!int.TryParse(txtEstoqueProduto.Text, out int estoque)
                || estoque < 0)
            {
                MessageBox.Show("Informe um estoque válido.");
                txtEstoqueProduto.Focus();
                return;
            }

            if (cmbFornecedor.SelectedValue is not int fornecedorId)
            {
                MessageBox.Show("Selecione um fornecedor.");
                cmbFornecedor.Focus();
                return;
            }

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
                buttonConfirmarCad.IsEnabled = false;

                if (!string.IsNullOrWhiteSpace(caminhoImagemSelecionada))
                {
                    FileInfo arquivo = new FileInfo(caminhoImagemSelecionada);

                    if (arquivo.Length > 5 * 1024 * 1024)
                    {
                        throw new ArgumentException(
                            "A imagem deve ter no máximo 5 MB.");
                    }

                    produto.ImagemProduto =
                        await File.ReadAllBytesAsync(caminhoImagemSelecionada);
                }

                int idGerado =
                    await produtoController.CadastrarProdutoAsync(produto);

                txtIdProduto.Text = idGerado.ToString();

                MessageBox.Show(
                    $"Produto cadastrado com sucesso!\nID: {idGerado}",
                    "Loja Virtual",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                PainelAdmin painel = new PainelAdmin();
                painel.Show();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Não foi possível cadastrar o produto:\n" + ex.Message,
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                buttonConfirmarCad.IsEnabled = true;
            }


        }

        private void buttonInserirImg_Click(
            object sender, RoutedEventArgs e)
        {

            OpenFileDialog janela = new OpenFileDialog
            {
                Title = "Selecionar imagem do produto",

                Filter = "Arquivos de imagem|*.jpg;*.jpeg;*.png;*.bmp",

                Multiselect = false
            };

            if (janela.ShowDialog() == true)
            {
                try
                {
                    BitmapImage imagem = new BitmapImage();

                    imagem.BeginInit();

                    imagem.UriSource = new Uri(
                        janela.FileName,
                        UriKind.Absolute);

                    imagem.CacheOption = BitmapCacheOption.OnLoad;

                    imagem.EndInit();

                    imagem.Freeze();

                    imgProduto.Source = imagem;

                    imgProduto.Visibility = Visibility.Visible;

                    txtSemImagem.Visibility = Visibility.Collapsed;

                    caminhoImagemSelecionada = janela.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Não foi possível carregar a imagem.\n\n" +
                        ex.Message,
                        "Erro",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void buttonLimparImg_Click(
            object sender, RoutedEventArgs e)
        {
            imgProduto.Source = null;

            imgProduto.Visibility = Visibility.Collapsed;

            txtSemImagem.Visibility = Visibility.Visible;

            caminhoImagemSelecionada = null;
        }

        private void VoltarPainel_Click(
            object sender, RoutedEventArgs e)
        {
            if (!SessaoUsuario.EhAdministrador)
            {
                MessageBox.Show(
                    "Acesso permitido apenas para administradores.",
                    "Acesso negado",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            PainelAdmin painel = new PainelAdmin();
            painel.Show();

            Close();
        }



    }
}
