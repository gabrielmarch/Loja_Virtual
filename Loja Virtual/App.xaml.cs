
using Loja_Virtual.data;
using System;
using System.Windows;

namespace Loja_Virtual
{
    public partial class App : Application
    {
        protected override void OnStartup(
            StartupEventArgs e)
        {
            try
            {
                DbInitializer.Inicializar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Não foi possível inicializar os dados.\n\n" +
                    "Verifique se o banco LojaVirtualDb existe " +
                    "e se as migrations foram aplicadas.\n\n" +
                    ex.Message,
                    "Erro de inicialização",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Shutdown();
                return;
            }

            base.OnStartup(e);
        }
    }
}
