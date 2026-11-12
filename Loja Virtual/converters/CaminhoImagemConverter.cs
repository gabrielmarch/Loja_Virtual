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
            if (value is not byte[] bytes || bytes.Length == 0)
                return null;

            try
            {
                using MemoryStream stream = new MemoryStream(bytes);

                BitmapImage imagem = new BitmapImage();

                imagem.BeginInit();
                imagem.CacheOption = BitmapCacheOption.OnLoad;
                imagem.DecodePixelWidth = 400;
                imagem.StreamSource = stream;
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
