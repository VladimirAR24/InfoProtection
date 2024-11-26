using System.Text;

namespace InfoProtection.Protection
{
    public class Kuznechik
    {
        private const int BLOCK_SIZE = 16; // длина блока

        //static byte[] key_1 = new byte[] { 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11, 0x00, 0xFF, 0xEE, 0xDD, 0xCC, 0xBB, 0xAA, 0x99, 0x88 };
        //static byte[] key_2 = new byte[] { 0xEF, 0xCD, 0xAB, 0x89, 0x67, 0x45, 0x23, 0x01, 0x10, 0x32, 0x54, 0x76, 0x98, 0xBA, 0xDC, 0xFE };
        static byte[] key_1 = System.Convert.FromHexString("7766554433221100ffeeddccbbaa9988");
        static byte[] key_2 = System.Convert.FromHexString("efcdab89674523011032547698badcfe");

        //static byte[] blk = System.Convert.FromHexString("1122334455667700ffeeddccbbaa9988");


        public static string KuznechikEncrypt(string text)
        {
            //string hexText = BitConverter.ToString(Encoding.UTF8.GetBytes(text)).Replace("-", "");
            //Генерация раундовых ключей
            GOST_Kuz_Expand_Key(key_1, key_2);
            byte[] encriptBlok = GOST_Kuz_Encrypt(Convert.FromHexString(text));
            Console.WriteLine(BitConverter.ToString(encriptBlok).Replace("-", ""));


            return $"{BitConverter.ToString(encriptBlok).Replace("-", "")} Зашифровано с помощью кузнечика"; // Заглушка
        }
        public static string KuznechikDecrypt(string text)
        {
            //Генерация раундовых ключей
            GOST_Kuz_Expand_Key(key_1, key_2);
            byte[] decriptBlok = GOST_Kuz_Decrypt(Convert.FromHexString(text));
            Console.WriteLine(BitConverter.ToString(decriptBlok).Replace("-", ""));
            return $"{BitConverter.ToString(decriptBlok).Replace("-", "")} Расшифровано с помощью кузнечика"; // Заглушка
        }

        // таблица прямого нелинейного преобразования
        private static readonly byte[] Pi = new byte[]
        {
        0xFC, 0xEE, 0xDD, 0x11, 0xCF, 0x6E, 0x31, 0x16,
        0xFB, 0xC4, 0xFA, 0xDA, 0x23, 0xC5, 0x04, 0x4D,
        0xE9, 0x77, 0xF0, 0xDB, 0x93, 0x2E, 0x99, 0xBA,
        0x17, 0x36, 0xF1, 0xBB, 0x14, 0xCD, 0x5F, 0xC1,
        0xF9, 0x18, 0x65, 0x5A, 0xE2, 0x5C, 0xEF, 0x21,
        0x81, 0x1C, 0x3C, 0x42, 0x8B, 0x01, 0x8E, 0x4F,
        0x05, 0x84, 0x02, 0xAE, 0xE3, 0x6A, 0x8F, 0xA0,
        0x06, 0x0B, 0xED, 0x98, 0x7F, 0xD4, 0xD3, 0x1F,
        0xEB, 0x34, 0x2C, 0x51, 0xEA, 0xC8, 0x48, 0xAB,
        0xF2, 0x2A, 0x68, 0xA2, 0xFD, 0x3A, 0xCE, 0xCC,
        0xB5, 0x70, 0x0E, 0x56, 0x08, 0x0C, 0x76, 0x12,
        0xBF, 0x72, 0x13, 0x47, 0x9C, 0xB7, 0x5D, 0x87,
        0x15, 0xA1, 0x96, 0x29, 0x10, 0x7B, 0x9A, 0xC7,
        0xF3, 0x91, 0x78, 0x6F, 0x9D, 0x9E, 0xB2, 0xB1,
        0x32, 0x75, 0x19, 0x3D, 0xFF, 0x35, 0x8A, 0x7E,
        0x6D, 0x54, 0xC6, 0x80, 0xC3, 0xBD, 0x0D, 0x57,
        0xDF, 0xF5, 0x24, 0xA9, 0x3E, 0xA8, 0x43, 0xC9,
        0xD7, 0x79, 0xD6, 0xF6, 0x7C, 0x22, 0xB9, 0x03,
        0xE0, 0x0F, 0xEC, 0xDE, 0x7A, 0x94, 0xB0, 0xBC,
        0xDC, 0xE8, 0x28, 0x50, 0x4E, 0x33, 0x0A, 0x4A,
        0xA7, 0x97, 0x60, 0x73, 0x1E, 0x00, 0x62, 0x44,
        0x1A, 0xB8, 0x38, 0x82, 0x64, 0x9F, 0x26, 0x41,
        0xAD, 0x45, 0x46, 0x92, 0x27, 0x5E, 0x55, 0x2F,
        0x8C, 0xA3, 0xA5, 0x7D, 0x69, 0xD5, 0x95, 0x3B,
        0x07, 0x58, 0xB3, 0x40, 0x86, 0xAC, 0x1D, 0xF7,
        0x30, 0x37, 0x6B, 0xE4, 0x88, 0xD9, 0xE7, 0x89,
        0xE1, 0x1B, 0x83, 0x49, 0x4C, 0x3F, 0xF8, 0xFE,
        0x8D, 0x53, 0xAA, 0x90, 0xCA, 0xD8, 0x85, 0x61,
        0x20, 0x71, 0x67, 0xA4, 0x2D, 0x2B, 0x09, 0x5B,
        0xCB, 0x9B, 0x25, 0xD0, 0xBE, 0xE5, 0x6C, 0x52,
        0x59, 0xA6, 0x74, 0xD2, 0xE6, 0xF4, 0xB4, 0xC0,
        0xD1, 0x66, 0xAF, 0xC2, 0x39, 0x4B, 0x63, 0xB6
        };

        // таблица обратного нелинейного преобразования
        private static readonly byte[] reverse_Pi = new byte[]
        {
        0xA5, 0x2D, 0x32, 0x8F, 0x0E, 0x30, 0x38, 0xC0,
        0x54, 0xE6, 0x9E, 0x39, 0x55, 0x7E, 0x52, 0x91,
        0x64, 0x03, 0x57, 0x5A, 0x1C, 0x60, 0x07, 0x18,
        0x21, 0x72, 0xA8, 0xD1, 0x29, 0xC6, 0xA4, 0x3F,
        0xE0, 0x27, 0x8D, 0x0C, 0x82, 0xEA, 0xAE, 0xB4,
        0x9A, 0x63, 0x49, 0xE5, 0x42, 0xE4, 0x15, 0xB7,
        0xC8, 0x06, 0x70, 0x9D, 0x41, 0x75, 0x19, 0xC9,
        0xAA, 0xFC, 0x4D, 0xBF, 0x2A, 0x73, 0x84, 0xD5,
        0xC3, 0xAF, 0x2B, 0x86, 0xA7, 0xB1, 0xB2, 0x5B,
        0x46, 0xD3, 0x9F, 0xFD, 0xD4, 0x0F, 0x9C, 0x2F,
        0x9B, 0x43, 0xEF, 0xD9, 0x79, 0xB6, 0x53, 0x7F,
        0xC1, 0xF0, 0x23, 0xE7, 0x25, 0x5E, 0xB5, 0x1E,
        0xA2, 0xDF, 0xA6, 0xFE, 0xAC, 0x22, 0xF9, 0xE2,
        0x4A, 0xBC, 0x35, 0xCA, 0xEE, 0x78, 0x05, 0x6B,
        0x51, 0xE1, 0x59, 0xA3, 0xF2, 0x71, 0x56, 0x11,
        0x6A, 0x89, 0x94, 0x65, 0x8C, 0xBB, 0x77, 0x3C,
        0x7B, 0x28, 0xAB, 0xD2, 0x31, 0xDE, 0xC4, 0x5F,
        0xCC, 0xCF, 0x76, 0x2C, 0xB8, 0xD8, 0x2E, 0x36,
        0xDB, 0x69, 0xB3, 0x14, 0x95, 0xBE, 0x62, 0xA1,
        0x3B, 0x16, 0x66, 0xE9, 0x5C, 0x6C, 0x6D, 0xAD,
        0x37, 0x61, 0x4B, 0xB9, 0xE3, 0xBA, 0xF1, 0xA0,
        0x85, 0x83, 0xDA, 0x47, 0xC5, 0xB0, 0x33, 0xFA,
        0x96, 0x6F, 0x6E, 0xC2, 0xF6, 0x50, 0xFF, 0x5D,
        0xA9, 0x8E, 0x17, 0x1B, 0x97, 0x7D, 0xEC, 0x58,
        0xF7, 0x1F, 0xFB, 0x7C, 0x09, 0x0D, 0x7A, 0x67,
        0x45, 0x87, 0xDC, 0xE8, 0x4F, 0x1D, 0x4E, 0x04,
        0xEB, 0xF8, 0xF3, 0x3E, 0x3D, 0xBD, 0x8A, 0x88,
        0xDD, 0xCD, 0x0B, 0x13, 0x98, 0x02, 0x93, 0x80,
        0x90, 0xD0, 0x24, 0x34, 0xCB, 0xED, 0xF4, 0xCE,
        0x99, 0x10, 0x44, 0x40, 0x92, 0x3A, 0x01, 0x26,
        0x12, 0x1A, 0x48, 0x68, 0xF5, 0x81, 0x8B, 0xC7,
        0xD6, 0x20, 0x0A, 0x08, 0x00, 0x4C, 0xD7, 0x74
        };

        // вектор линейного преобразования
        private static readonly byte[] l_vec = new byte[]
        {
        1, 148, 32, 133, 16, 194, 192, 1,
        251, 1, 192, 194, 16, 133, 32, 148
        };

        // массив для хранения констант
        private static readonly byte[][] iter_C = new byte[32][];
        // массив для хранения ключей
        private static readonly byte[][] iter_key = new byte[10][];

        // X function
        private static byte[] GOST_Kuz_X(byte[] a, byte[] b)
        {
            byte[] c = new byte[BLOCK_SIZE];
            for (int i = 0; i < BLOCK_SIZE; i++)
            {
                c[i] = (byte)(a[i] ^ b[i]);
            }
            return c;
        }

        // S function
        private static byte[] GOST_Kuz_S(byte[] inData)
        {
            byte[] outData = new byte[inData.Length];
            for (int i = 0; i < BLOCK_SIZE; i++)
            {
                int data = inData[i];
                if (data < 0)
                {
                    data += 256;
                }
                outData[i] = Pi[data];
            }
            return outData;
        }

        // Multiplication in Galois field
        private static byte GOST_Kuz_GF_mul(byte a, byte b)
        {
            byte c = 0;
            byte hi_bit;
            for (int i = 0; i < 8; i++)
            {
                if ((b & 1) == 1)
                    c ^= a;
                hi_bit = (byte)(a & 0x80);
                a <<= 1;
                if (hi_bit != 0)
                    a ^= 0xc3; // polynomial x^8+x^7+x^6+x+1
                b >>= 1;
            }
            return c;
        }

        // R function shifts data and implements the equation for L-function calculation
        private static byte[] GOST_Kuz_R(byte[] state)
        {
            byte a_15 = 0;
            byte[] internalByte = new byte[16];
            for (int i = 15; i >= 0; i--)
            {
                if (i == 0)
                    internalByte[15] = state[i];
                else
                    internalByte[i - 1] = state[i];
                a_15 ^= GOST_Kuz_GF_mul(state[i], l_vec[i]);
            }
            internalByte[15] = a_15;
            return internalByte;
        }

        private static byte[] GOST_Kuz_L(byte[] in_data)
        {
            byte[] internalByte = (byte[])in_data.Clone();
            for (int i = 0; i < 16; i++)
            {
                internalByte = GOST_Kuz_R(internalByte);
            }
            return internalByte;
        }

        

        // S^(-1) function
        private static byte[] GOST_Kuz_reverse_S(byte[] in_data)
        {
            byte[] out_data = new byte[in_data.Length];
            for (int i = 0; i < BLOCK_SIZE; i++)
            {
                int data = in_data[i];
                if (data < 0)
                {
                    data += 256;
                }
                out_data[i] = reverse_Pi[data];
            }
            return out_data;
        }

        private static byte[] GOST_Kuz_reverse_R(byte[] state)
        {
            byte a_0 = state[15];
            byte[] internalByte = new byte[16];
            for (int i = 1; i < 16; i++)
            {
                internalByte[i] = state[i - 1];
                a_0 ^= GOST_Kuz_GF_mul(internalByte[i], l_vec[i]);
            }
            internalByte[0] = a_0;
            return internalByte;
        }

        private static byte[] GOST_Kuz_reverse_L(byte[] in_data)
        {
            byte[] internalByte = in_data;
            for (int i = 0; i < 16; i++)
                internalByte = GOST_Kuz_reverse_R(internalByte);
            return internalByte;
        }

        // Function to calculate constants
        private static void GOST_Kuz_Get_C()
        {
            byte[][] iter_num = new byte[32][];
            for (int i = 0; i < 32; i++)
            {
                iter_num[i] = new byte[16];
                iter_num[i][0] = (byte)(i + 1);
            }
            for (int i = 0; i < 32; i++)
            {
                iter_C[i] = GOST_Kuz_L(iter_num[i]);
            }
        }

        // Function performing Feistel cell transformations
        private static byte[][] GOST_Kuz_F(byte[] in_key_1, byte[] in_key_2, byte[] iter_const)
        {
            byte[] internalByte = GOST_Kuz_X(in_key_1, iter_const);
            internalByte = GOST_Kuz_S(internalByte);
            internalByte = GOST_Kuz_L(internalByte);
            byte[] out_key_1 = GOST_Kuz_X(internalByte, in_key_2);
            return new byte[][] { out_key_1, in_key_1 };
        }

        // Function to calculate round keys
        public static void GOST_Kuz_Expand_Key(byte[] key_1, byte[] key_2)
        {
            GOST_Kuz_Get_C();
            iter_key[0] = key_1;
            iter_key[1] = key_2;
            byte[][] iter12 = new byte[][] { key_1, key_2 };
            byte[][] iter34;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    iter34 = GOST_Kuz_F(iter12[0], iter12[1], iter_C[j + 8 * i]);
                    iter12 = iter34;
                }
                iter_key[2 * i + 2] = iter12[0];
                iter_key[2 * i + 3] = iter12[1];
            }
        }

        // Function to encrypt a block
        public static byte[] GOST_Kuz_Encrypt(byte[] blk)
        {
            byte[] out_blk = blk;
            for (int i = 0; i < 9; i++)
            {
                out_blk = GOST_Kuz_X(iter_key[i], out_blk);
                out_blk = GOST_Kuz_S(out_blk);
                out_blk = GOST_Kuz_L(out_blk);
            }
            out_blk = GOST_Kuz_X(out_blk, iter_key[9]);
            return out_blk;
        }

        // Function to decrypt a block
        public static byte[] GOST_Kuz_Decrypt(byte[] blk)
        {
            byte[] out_blk = GOST_Kuz_X(blk, iter_key[9]);
            for (int i = 8; i >= 0; i--)
            {
                out_blk = GOST_Kuz_reverse_L(out_blk);
                out_blk = GOST_Kuz_reverse_S(out_blk);
                out_blk = GOST_Kuz_X(iter_key[i], out_blk);
            }
            return out_blk;
        }
    }
}
