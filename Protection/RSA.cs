using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System.Text;
using System.Diagnostics;

namespace InfoProtection.Protection
{
    public static class RSA
    {
        public static string RsaEncrypt(string originalMessage)
        {
            var keyPair = GenerateRsaKeyPair();
            var publicKey = (RsaKeyParameters)keyPair.Public;
            var privateKey = (RsaKeyParameters)keyPair.Private;

            byte[] originalData = Encoding.UTF8.GetBytes(originalMessage);

            // Шифрование сообщения
            byte[] encryptedData = Encrypt(originalData, publicKey);

            // Выводим зашифрованные данные в консоль
            Console.WriteLine("Encrypted Data: " + Convert.ToBase64String(encryptedData));

            return $"{Convert.ToBase64String(encryptedData)}. Засшифровано с помощью рса. Публичный ключ ({publicKey}). Приватный ключ ({privateKey})"; // Заглушка
        }
        public static string RsaDecrypt(string text, RsaKeyParameters privateKey)
        {
            return $"{Decrypt(Convert.FromHexString(text), privateKey)} Расшифровано с помощью рса"; // Заглушка
        }
        public static AsymmetricCipherKeyPair GenerateRsaKeyPair()
        {
            var keyGen = new RsaKeyPairGenerator();
            keyGen.Init(new KeyGenerationParameters(new SecureRandom(), 16384));
            return keyGen.GenerateKeyPair();
        }

        public static byte[] Encrypt(byte[] data, RsaKeyParameters publicKey)
        {
            var encryptEngine = new Org.BouncyCastle.Crypto.Engines.RsaEngine();
            encryptEngine.Init(true, publicKey);
            return ProcessDataInChunks(data, encryptEngine);
        }

        public static byte[] Decrypt(byte[] encryptedData, RsaKeyParameters privateKey)
        {
            var decryptEngine = new Org.BouncyCastle.Crypto.Engines.RsaEngine();
            decryptEngine.Init(false, privateKey);
            return ProcessDataInChunks(encryptedData, decryptEngine);
        }

        private static byte[] ProcessDataInChunks(byte[] data, IAsymmetricBlockCipher engine)
        {
            var inputChunks = SplitIntoChunks(data, engine.GetInputBlockSize());
            var output = inputChunks.Select(chunk => engine.ProcessBlock(chunk, 0, chunk.Length)).ToList();
            return CombineChunks(output);
        }

        private static IEnumerable<byte[]> SplitIntoChunks(byte[] data, int chunkSize)
        {
            for (int i = 0; i < data.Length; i += chunkSize)
                yield return data.Skip(i).Take(chunkSize).ToArray();
        }

        private static byte[] CombineChunks(IEnumerable<byte[]> chunks)
        {
            return chunks.SelectMany(chunk => chunk).ToArray();
        }

    }
}
