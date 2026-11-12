using System;
using System.Collections.Generic;
using System.Text;
using Loja_Virtual.services;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Loja_Virtual.converters
{
    public class CaminhoImagemConverter : IValueConverter
    {
        public object? Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            if (value is not string nomeArquivo ||
                string.IsNullOrWhiteSpace(nomeArquivo))
            {
                return null;
            }

            string caminho =
                ImagemProdutoService.ObterCaminhoCompleto(nomeArquivo);

            if (!File.Exists(caminho))
                return null;

            try
            {
                BitmapImage imagem = new BitmapImage();

                imagem.BeginInit();
                imagem.UriSource = new Uri(caminho);
                imagem.CacheOption = BitmapCacheOption.OnLoad;
                imagem.DecodePixelWidth = 400;
                imagem.EndInit();
                imagem.Freeze();

                return imagem;
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
