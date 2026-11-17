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
using Loja_Virtual.controll;
using Loja_Virtual.model;
using Loja_Virtual.services;
using Microsoft.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;

namespace Loja_Virtual.view
{
    public partial class GerenciarCupons : Window
    {
        private readonly CupomController _controller = new();

        private int? _cupomSelecionadoId;

        private bool _ocupado = false;

        private readonly CultureInfo _cultura =
            CultureInfo.GetCultureInfo("pt-BR");

        public GerenciarCupons()
        {
            InitializeComponent();

            LimparFormulario();
        }

        private async void Window_Loaded(
            object sender, RoutedEventArgs e)
        {
            if (!SessaoUsuario.EhAdministrador)
            {
                MessageBox.Show(
                    "Acesso permitido apenas para administradores.",
                    "Acesso negado",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                Close();
                return;
            }

            await CarregarCuponsAsync();
        }

        private async Task CarregarCuponsAsync()
        {
            try
            {
                ListaCupons.ItemsSource =
                    await _controller.ListarAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Erro ao consultar cupons:\n" + ex.Message,
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ListaCupons_SelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            if (ListaCupons.SelectedItem is not Cupom cupom)
            {
                _cupomSelecionadoId = null;
                BtnAlternarStatus.IsEnabled = false;
                return;
            }

            _cupomSelecionadoId = cupom.idCupom;

            TxtCodigo.Text = cupom.CodigoCupom;

            TxtDesconto.Text =
                cupom.Desconto.ToString("0.##", _cultura);

            DpValidade.SelectedDate =
                cupom.DataValidade;

            ChkAtivo.IsChecked = cupom.ativo;

            TituloFormulario.Text = "Editar Cupom";
            BtnSalvar.Content = "Salvar Alterações";

            BtnAlternarStatus.IsEnabled = true;

            BtnAlternarStatus.Content =
                cupom.ativo ? "Desativar Cupom" : "Ativar Cupom";
        }

        private async void SalvarCupom_Click(
            object sender, RoutedEventArgs e)
        {
            if (_ocupado)
                return;

            if (string.IsNullOrWhiteSpace(TxtCodigo.Text))
            {
                MessageBox.Show("Informe o código do cupom.");
                return;
            }

            if (!decimal.TryParse(
                    TxtDesconto.Text,
                    NumberStyles.Number,
                    _cultura,
                    out decimal desconto))
            {
                MessageBox.Show(
                    "Informe um percentual de desconto válido.",
                    "Validação",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (DpValidade.SelectedDate == null)
            {
                MessageBox.Show(
                    "Informe a data de validade.",
                    "Validação",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var cupom = new Cupom
            {
                idCupom = _cupomSelecionadoId ?? 0,
                CodigoCupom = TxtCodigo.Text.Trim(),
                Desconto = desconto,
                DataValidade = DpValidade.SelectedDate.Value,
                ativo = ChkAtivo.IsChecked == true
            };

            _ocupado = true;
            BtnSalvar.IsEnabled = false;

            try
            {
                if (_cupomSelecionadoId.HasValue)
                {
                    bool atualizado =
                        await _controller.AtualizarAsync(cupom);

                    if (!atualizado)
                    {
                        MessageBox.Show(
                            "O cupom selecionado não foi encontrado.");
                        return;
                    }

                    MessageBox.Show(
                        "Cupom atualizado com sucesso!",
                        "Sucesso",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    int id =
                        await _controller.CadastrarAsync(cupom);

                    MessageBox.Show(
                        $"Cupom cadastrado com sucesso! ID: {id}",
                        "Sucesso",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }

                await CarregarCuponsAsync();
                LimparFormulario();
            }
            catch (SqlException ex)
                when (ex.Number == 2601 || ex.Number == 2627)
            {
                MessageBox.Show(
                    "Já existe um cupom com esse código.",
                    "Código duplicado",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Não foi possível salvar o cupom:\n" +
                    ex.Message,
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                _ocupado = false;
                BtnSalvar.IsEnabled = true;
            }
        }

        private async void AlternarStatus_Click(
            object sender, RoutedEventArgs e)
        {
            if (_ocupado ||
                ListaCupons.SelectedItem is not Cupom cupom)
            {
                return;
            }

            bool novoStatus = !cupom.ativo;

            string acao = novoStatus ? "ativar" : "desativar";

            MessageBoxResult resposta = MessageBox.Show(
                $"Deseja realmente {acao} o cupom " +
                $"{cupom.CodigoCupom}?",
                "Confirmar alteração",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resposta != MessageBoxResult.Yes)
                return;

            _ocupado = true;
            BtnAlternarStatus.IsEnabled = false;

            try
            {
                bool alterado =
                    await _controller.AlterarStatusAsync(
                        cupom.idCupom, novoStatus);

                if (!alterado)
                {
                    MessageBox.Show(
                        "Cupom não encontrado.",
                        "Aviso",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                MessageBox.Show(
                    novoStatus
                        ? "Cupom ativado com sucesso!"
                        : "Cupom desativado com sucesso!",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                await CarregarCuponsAsync();
                LimparFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Não foi possível alterar o status:\n" +
                    ex.Message,
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                _ocupado = false;

                BtnAlternarStatus.IsEnabled =
                    ListaCupons.SelectedItem is Cupom;
            }
        }

        private void LimparFormulario()
        {
            ListaCupons.SelectedItem = null;

            _cupomSelecionadoId = null;

            TxtCodigo.Clear();
            TxtDesconto.Clear();

            DpValidade.SelectedDate =
                DateTime.Today.AddMonths(1);

            ChkAtivo.IsChecked = true;

            TituloFormulario.Text = "Cadastrar Novo Cupom";
            BtnSalvar.Content = "Cadastrar Cupom";

            BtnAlternarStatus.IsEnabled = false;
            BtnAlternarStatus.Content = "Ativar / Desativar";
        }

        private void NovoCupom_Click(
            object sender, RoutedEventArgs e)
        {
            if (_ocupado)
                return;

            LimparFormulario();
        }

        private async void AtualizarLista_Click(
            object sender, RoutedEventArgs e)
        {
            if (_ocupado)
                return;

            await CarregarCuponsAsync();
            LimparFormulario();
        }

        private void VoltarPainel_Click(
            object sender, RoutedEventArgs e)
        {
            PainelAdmin painel = new PainelAdmin();

            painel.Show();
            Close();
        }
    }
}
