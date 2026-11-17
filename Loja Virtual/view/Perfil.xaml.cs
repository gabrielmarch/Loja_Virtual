using Loja_Virtual.controll;
using Loja_Virtual.data;
using Loja_Virtual.model;
using Loja_Virtual.services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class Perfil : Window
    {
        private bool menuVisivel = true;

        public Perfil()
        {
            InitializeComponent();
        }

        
        private async void Window_Loaded(
            object sender, RoutedEventArgs e)
        {
            if (!SessaoUsuario.EstaAutenticado ||
                SessaoUsuario.ClienteId == null)
            {
                MessageBox.Show(
                    "Você precisa estar conectado para acessar o perfil.",
                    "Acesso restrito",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                Login login = new Login();
                login.Show();
                Close();
                return;
            }

            try
            {
                int clienteId = SessaoUsuario.ClienteId.Value;

                using var banco = new LojaVirtualDbContext();

                Cliente? cliente = await banco.Clientes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == clienteId);

                if (cliente == null || !cliente.Ativo)
                {
                    SessaoUsuario.Encerrar();

                    MessageBox.Show(
                        "Cliente não encontrado ou inativo.",
                        "Perfil",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    Login login = new Login();
                    login.Show();
                    Close();
                    return;
                }

                
                NomeUsuario.Text = cliente.Nome;
                EmailUsuario.Text = cliente.Email;

                TelefoneUsuario.Text =
                    string.IsNullOrWhiteSpace(cliente.Telefone)
                    ? "Não informado"
                    : cliente.Telefone;

                EnderecoUsuario.Text =
                    string.IsNullOrWhiteSpace(cliente.Endereco)
                    ? "Não informado"
                    : cliente.Endereco;

                
                if (cliente is PessoaFisica)
                {
                    TipoClienteTexto.Text = "Pessoa Física";
                }
                else if (cliente is PessoaJuridica)
                {
                    TipoClienteTexto.Text = "Pessoa Jurídica";
                }

                
                int totalPedidos = await banco.Clientes
                    .Where(c => c.Id == clienteId)
                    .Select(c => c.Pedidos.Count)
                    .SingleAsync();

                QuantidadePedidos.Text = totalPedidos.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Não foi possível carregar o perfil.\n" +
                    ex.Message,
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        
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

        private void VoltarHome_Click(
            object sender, RoutedEventArgs e)
        {
            TelaPrincipal principal = new TelaPrincipal();

            principal.Show();
            Close();
        }


        private void SairConta_Click(
            object sender, RoutedEventArgs e)
        {
            MessageBoxResult resposta = MessageBox.Show(
                "Deseja realmente sair da sua conta?",
                "Confirmar saída",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resposta != MessageBoxResult.Yes)
                return;
            CarrinhoServiço.Limpar();
            SessaoUsuario.Encerrar();
            Login login = new Login();
            login.Show();

            this.Close();
        }



    }
}

