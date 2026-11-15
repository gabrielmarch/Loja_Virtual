using Loja_Virtual.controll;
using Loja_Virtual.model;
using Loja_Virtual.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Loja_Virtual.view
{
    public partial class AdicionarCartao : Window
    {
        private bool menuVisivel = true;

        private bool atualizandoValidade = false;
        private bool atualizandoNumero = false;
        private bool atualizandoNome = false;
        private bool atualizandoCvv = false;

        public AdicionarCartao()
        {
            InitializeComponent();
        }

        // ABRIR E FECHAR O MENU HAMBÚRGUER
        private void buttonMenu_Click(object sender, RoutedEventArgs e)
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

        // =========================================
        // NÚMERO DO CARTÃO
        // =========================================

        private void TxtNumeroCartao_PreviewTextInput(
            object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }

        private void TxtNumeroCartao_TextChanged(
            object sender, TextChangedEventArgs e)
        {
            if (atualizandoNumero)
                return;

            string numeros = new string(
                TxtNumeroCartao.Text.Where(char.IsDigit).ToArray());

            numeros = numeros.Length > 8
                ? numeros.Substring(0, 8)
                : numeros;

            if (TxtNumeroCartao.Text != numeros)
            {
                atualizandoNumero = true;

                TxtNumeroCartao.Text = numeros;
                TxtNumeroCartao.CaretIndex = numeros.Length;

                atualizandoNumero = false;
            }
        }

        // =========================================
        // NOME NO CARTÃO
        // =========================================

        private void TxtNomeCartao_PreviewTextInput(
            object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(c =>
                char.IsLetter(c) || char.IsWhiteSpace(c));
        }

        private void TxtNomeCartao_TextChanged(
            object sender, TextChangedEventArgs e)
        {
            if (atualizandoNome)
                return;

            string nome = new string(
                TxtNomeCartao.Text
                    .Where(c => char.IsLetter(c) || char.IsWhiteSpace(c))
                    .ToArray());

            nome = nome.ToUpperInvariant();

            if (nome.Length > 70)
                nome = nome.Substring(0, 70);

            if (TxtNomeCartao.Text != nome)
            {
                atualizandoNome = true;

                TxtNomeCartao.Text = nome;
                TxtNomeCartao.CaretIndex = nome.Length;

                atualizandoNome = false;
            }
        }

        // =========================================
        // VALIDADE DO CARTÃO
        // =========================================

        private void TxtValidade_PreviewTextInput(
            object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }

        private void TxtValidade_TextChanged(
            object sender, TextChangedEventArgs e)
        {
            if (atualizandoValidade)
                return;

            string numeros = new string(
                TxtValidade.Text.Where(char.IsDigit).ToArray());

            if (numeros.Length > 4)
                numeros = numeros.Substring(0, 4);

            string validade = numeros;

            // Insere a barra automaticamente após o mês
            if (numeros.Length > 2)
            {
                validade = numeros.Substring(0, 2)
                         + "/"
                         + numeros.Substring(2);
            }

            if (TxtValidade.Text != validade)
            {
                atualizandoValidade = true;

                TxtValidade.Text = validade;
                TxtValidade.CaretIndex = validade.Length;

                atualizandoValidade = false;
            }
        }

        // =========================================
        // CVV
        // =========================================

        private void TxtCvv_PreviewTextInput(
            object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }

        private void TxtCvv_PasswordChanged(
            object sender, RoutedEventArgs e)
        {
            if (atualizandoCvv)
                return;

            string numeros = new string(
                TxtCvv.Password.Where(char.IsDigit).ToArray());

            if (numeros.Length > 3)
                numeros = numeros.Substring(0, 3);

            if (TxtCvv.Password != numeros)
            {
                atualizandoCvv = true;

                TxtCvv.Password = numeros;

                atualizandoCvv = false;
            }
        }

        // =========================================
        // VALIDAR VALIDADE DO CARTÃO
        // =========================================

        private bool ValidadeCartaoValida()
        {
            string validade = TxtValidade.Text;

            if (!Regex.IsMatch(validade, @"^(0[1-9]|1[0-2])/\d{2}$"))
                return false;

            string[] partes = validade.Split('/');

            int mes = int.Parse(partes[0]);
            int ano = 2000 + int.Parse(partes[1]);

            int ultimoDia = DateTime.DaysInMonth(ano, mes);

            DateTime dataValidade =
                new DateTime(ano, mes, ultimoDia);

            return dataValidade.Date >= DateTime.Today;
        }

        // =========================================
        // SALVAR / VALIDAR CARTÃO
        // =========================================

        private void BtnSalvarCartao_Click(object sender, RoutedEventArgs e)
        {
            string numero = TxtNumeroCartao.Text.Trim();
            string nome = TxtNomeCartao.Text.Trim();
            string validade = TxtValidade.Text.Trim();
            string cvv = TxtCvv.Password;

            // VALIDAÇÃO DO NÚMERO
            if (!Regex.IsMatch(numero, @"^\d{8}$"))
            {
                MessageBox.Show(
                    "O número do cartão deve conter exatamente 8 números.",
                    "Cartão inválido",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                TxtNumeroCartao.Focus();
                return;
            }

            // VALIDAÇÃO DO NOME
            if (string.IsNullOrWhiteSpace(nome) ||
                nome.Length > 70 ||
                !nome.All(c =>
                    char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                MessageBox.Show(
                    "Informe o nome no cartão utilizando somente letras e espaços, com até 70 caracteres.",
                    "Nome inválido",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                TxtNomeCartao.Focus();
                return;
            }

            // VALIDAÇÃO DA VALIDADE
            if (!ValidadeCartaoValida())
            {
                MessageBox.Show(
                    "Informe uma validade válida no formato MM/AA. O cartão não pode estar vencido.",
                    "Validade inválida",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                TxtValidade.Focus();
                return;
            }

            // VALIDAÇÃO DO CVV
            if (!Regex.IsMatch(cvv, @"^\d{3}$"))
            {
                MessageBox.Show(
                    "O CVV deve conter exatamente 3 números.",
                    "CVV inválido",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                TxtCvv.Focus();
                return;
            }


            try
            {
                // Verifica se existe uma compra preparada
                if (!FinalizacaoCompraService.CompraPreparada)
                {
                    MessageBox.Show(
                        "Não existe uma compra para finalizar.",
                        "Atenção",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                // Finaliza e grava a compra no banco
                PedidoController pedidoController =
                    new PedidoController();

                pedidoController.FinalizarCompra(
                    SessaoUsuario.ClienteId);

                // A compra foi gravada com sucesso.
                // Agora podemos limpar o carrinho.
                CarrinhoService.Itens.Clear();

                FinalizacaoCompraService.Limpar();

                // Limpa os dados sensíveis do cartão
                TxtNumeroCartao.Clear();
                TxtNomeCartao.Clear();
                TxtValidade.Clear();
                TxtCvv.Clear();

                MessageBox.Show(
                    "Pagamento confirmado!\n\n" +
                    "Seu pedido foi registrado com sucesso.",
                    "Compra realizada",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                // Retorna para a tela principal
                TelaPrincipal telaPrincipal =
                    new TelaPrincipal(SessaoUsuario.ClienteId);

                telaPrincipal.Show();

                // Fecha a tela de pagamento
                this.Close();
            }
            catch (Exception ex)
            {
                string detalhes = ex.ToString();

                Exception? erroInterno = ex;

                while (erroInterno.InnerException != null)
                {
                    erroInterno = erroInterno.InnerException;
                    detalhes += "\n\nDETALHE INTERNO:\n" +
                                erroInterno.Message;
                }

                MessageBox.Show(
                    "Não foi possível finalizar a compra.\n\n" +
                    detalhes,
                    "Erro ao finalizar compra",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

        }
    }
}