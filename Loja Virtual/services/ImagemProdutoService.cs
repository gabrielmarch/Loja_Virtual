using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Loja_Virtual.services
{
    public static class ImagemProdutoService
    {
        private static readonly string PastaImagens =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "LojaVirtual",
                "ImagensProdutos");

        public static string SalvarImagem(string caminhoOriginal)
        {
            if (!File.Exists(caminhoOriginal))
                throw new FileNotFoundException(
                    "A imagem selecionada não foi encontrada.");

            Directory.CreateDirectory(PastaImagens);

            string extensao = Path.GetExtension(caminhoOriginal)
                .ToLowerInvariant();

            if (extensao != ".png" &&
                extensao != ".jpg" &&
                extensao != ".jpeg" &&
                extensao != ".bmp")
            {
                throw new ArgumentException(
                    "Formato de imagem não permitido.");
            }

            string nomeArquivo = Guid.NewGuid().ToString("N")
                                 + extensao;

            string destino = Path.Combine(PastaImagens, nomeArquivo);

            File.Copy(caminhoOriginal, destino);

            
            return nomeArquivo;
        }

        
        public static string ObterCaminhoCompleto(string nomeArquivo)
        {
            return Path.Combine(
                PastaImagens,
                Path.GetFileName(nomeArquivo));
        }

    
        public static void ExcluirImagem(string nomeArquivo)
        {
            string caminho = ObterCaminhoCompleto(nomeArquivo);

            if (File.Exists(caminho))
                File.Delete(caminho);
        }
    }
}
