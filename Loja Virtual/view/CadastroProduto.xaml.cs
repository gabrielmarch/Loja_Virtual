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
using System.Windows.Media.Imaging;
using System.Globalization;
using Loja_Virtual.controll;
using Loja_Virtual.model;
using Loja_Virtual.services;

namespace Loja_Virtual.view
{
    public partial class CadastroProduto : Window
    {
        private bool menuVisivel = true;
        private string? caminhoImagemSelecionada = null;

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

            string? imagemCopiada = null;

            try
            {
                buttonConfirmarCad.IsEnabled = false;

                // Salvar a imagem selecionada na pasta do aplicativo
                if (!string.IsNullOrWhiteSpace(caminhoImagemSelecionada))
                {
                    imagemCopiada =
                        ImagemProdutoService.SalvarImagem(caminhoImagemSelecionada);

                    // O banco guarda o nome do arquivo
                    produto.CaminhoImagem = imagemCopiada;
                }

                // Cadastrar o produto com sua imagem
                int idGerado =
                    await produtoController.CadastrarProdutoAsync(produto);

                txtIdProduto.Text = idGerado.ToString();

                MessageBox.Show(
                    $"Produto cadastrado com sucesso!\nID: {idGerado}",
                    "Loja Virtual",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                TelaPrincipal principal = new TelaPrincipal();
                principal.Show();

                this.Close();
            }
            catch (Exception ex)
            {
                // Limpar a imagem copiada se o cadastro falhou
                if (imagemCopiada != null)
                {
                    try
                    {
                        ImagemProdutoService.ExcluirImagem(imagemCopiada);
                    }
                    catch
                    {
                        // Uma falha de limpeza não substitui o erro principal
                    }
                }

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

        private void buttonInserirImg_Click(
            object sender, RoutedEventArgs e)
        {
            // Abre a janela para selecionar uma imagem
            OpenFileDialog janela = new OpenFileDialog
            {
                Title = "Selecionar imagem do produto",

                Filter = "Arquivos de imagem|*.jpg;*.jpeg;*.png;*.bmp",

                Multiselect = false
            };

            // Verifica se o usuário selecionou um arquivo
            if (janela.ShowDialog() == true)
            {
                try
                {
                    // Carrega a imagem na memória
                    BitmapImage imagem = new BitmapImage();

                    imagem.BeginInit();

                    imagem.UriSource = new Uri(
                        janela.FileName,
                        UriKind.Absolute);

                    imagem.CacheOption = BitmapCacheOption.OnLoad;

                    imagem.EndInit();

                    imagem.Freeze();

                    // Exibe a imagem no formulário
                    imgProduto.Source = imagem;

                    imgProduto.Visibility = Visibility.Visible;

                    // Esconde o texto padrão
                    txtSemImagem.Visibility = Visibility.Collapsed;

                    // Armazena o caminho selecionado
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
            // Remove a imagem da interface
            imgProduto.Source = null;

            imgProduto.Visibility = Visibility.Collapsed;

            // Exibe novamente o texto padrão
            txtSemImagem.Visibility = Visibility.Visible;

            // Limpa o caminho da imagem
            caminhoImagemSelecionada = null;
        }




    }
}
