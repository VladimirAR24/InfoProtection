
using InfoProtection.Servises;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Digests;
using System.Security.Cryptography;
using System.Text;

namespace InfoProtection.Protection
{
    //static мб
    public class HashMethods
    {
        public static string GenerateSalt(int size = 16)
        {
            var rng = new RNGCryptoServiceProvider();
            var buffer = new byte[size];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

        //https://habr.com/ru/articles/708774/
        // Официальная ру либа, но нужны деньги
        //public static string HashPasswordUsingStreebog(string password, string salt)
        //{
        //    // Пример добавления соли к паролю и хеширования с использованием Стрибог (или другого алгоритма)
        //    var combined = password + salt; // Добавляем соль к паролю
        //    byte[] inputBytes = Encoding.UTF8.GetBytes(combined);

        //    // Используем алгоритм хеширования Стрибог
        //    using (var hasher = new Gost3411_2012_256CryptoServiceProvider())
        //    {
        //        var hashedBytes = hasher.ComputeHash(inputBytes);
        //        return Convert.ToBase64String(hashedBytes);
        //    }
        //}


        public static string HashPasswordUsingStreebog(string password, string salt)
        {
            //    var gost3411 = new Gost3411_2012_256CryptoServiceProvider();
            var combined = Encoding.UTF8.GetBytes(password + salt);
            var digest = new Gost3411_2012_256Digest();  // ГОСТ Р 34.11-2012 с длиной вывода 256 бит
            byte[] hash = new byte[digest.GetDigestSize()];
            digest.BlockUpdate(combined, 0, combined.Length);
            digest.DoFinal(hash, 0);

            return Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(string inputPassword, string dbPassword, string salt)
        {
            // Получаем соль и хешируем введённый пароль
            string hashedPassword = HashPasswordUsingStreebog(inputPassword, salt);

            // Сравниваем хеши
            return hashedPassword == dbPassword;
        }
    }
}
