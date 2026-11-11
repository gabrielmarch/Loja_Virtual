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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Loja_Virtual.model;

using Loja_Virtual.view;

namespace Loja_Virtual.components
{
    public partial class MenuLateral : UserControl
    {
        public MenuLateral()
        {
            InitializeComponent();
        }

        private void NavegarMenu_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button botao)
                return;

            string destino = botao.Tag?.ToString() ?? "";

            Window? janelaAtual = Window.GetWindow(this);
            Window? novaTela = null;

            switch (destino)
            {
                case "Home":
                    if (janelaAtual is TelaPrincipal) return;
                    novaTela = new TelaPrincipal(SessaoUsuario.ClienteId);
                    break;

                case "Carrinho":
                    if (janelaAtual is CarrinhoCompras) return;
                    novaTela = new CarrinhoCompras();
                    break;

                case "Pedidos":
                    if (janelaAtual is StatusPedido) return;
                    novaTela = new StatusPedido();
                    break;

                case "Produtos":
                    if (janelaAtual is CadastroProduto) return;
                    novaTela = new CadastroProduto();
                    break;

                case "Suporte":
                    if (janelaAtual is SuporteCliente) return;
                    novaTela = new SuporteCliente();
                    break;
                case "Pagamento":
                    if (janelaAtual is MetodoPagamento) return;
                    novaTela = new MetodoPagamento();
                    break;

                case "Perfil":
                case "Anunciar":
                case "Cupons":
                    MessageBox.Show(
                        $"A interface {destino} ainda não foi configurada.",
                        "Loja Virtual",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;

                default:
                    return;
            }

            if (novaTela != null)
            {
                novaTela.Show();

                janelaAtual?.Close();
            }
        }
    }
}
