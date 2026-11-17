using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Loja_Virtual.services
{
    public static class SenhaService
    {
        private const int Iteracoes = 600000;
        private const int TamanhoSalt = 16;
        private const int TamanhoHash = 32;

        public static string GerarHash(string senha)
        {
            byte[] salt = RandomNumberGenerator
                .GetBytes(TamanhoSalt);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                senha,
                salt,
                Iteracoes,
                HashAlgorithmName.SHA256,
                TamanhoHash);

            return $"PBKDF2-SHA256${Iteracoes}$" +
                   $"{Convert.ToBase64String(salt)}$" +
                   $"{Convert.ToBase64String(hash)}";
        }

        public static bool VerificarSenha(
            string senha, string senhaArmazenada)
        {
            try
            {
                string[] partes = senhaArmazenada.Split('$');

                if (partes.Length != 4 ||
                    partes[0] != "PBKDF2-SHA256")
                    return false;

                int iteracoes = int.Parse(partes[1]);

                if (iteracoes < 100000 || iteracoes > 2000000)
                    return false;

                byte[] salt = Convert.FromBase64String(partes[2]);
                byte[] hashOriginal = Convert.FromBase64String(partes[3]);

                if (salt.Length != TamanhoSalt ||
                    hashOriginal.Length != TamanhoHash)
                    return false;

                byte[] hashInformado = Rfc2898DeriveBytes.Pbkdf2(
                    senha,
                    salt,
                    iteracoes,
                    HashAlgorithmName.SHA256,
                    hashOriginal.Length);

                return CryptographicOperations.FixedTimeEquals(
                    hashOriginal, hashInformado);
            }
            catch
            {
                return false;
            }
        }
    }
}

