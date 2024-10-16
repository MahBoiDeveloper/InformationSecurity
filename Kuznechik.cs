using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;

namespace InformationSecurity
{
    /// <summary>
    /// Подсказки по алгоритму:</br>
    /// 1. <a href="https://habr.com/ru/articles/459004/">https://habr.com/ru/articles/459004/</a><br/>
    /// 2. <a href="https://www.cyberforum.ru/post11934437.html">https://www.cyberforum.ru/post11934437.html</a>
    /// <br/>
    /// Релизован режим простой замены (Electronic Codebook, ECB).
    /// </summary>
    class Kuznechik
    {
        #region Constants
        /// <summary>
        /// Длина блока шифрования 128 бит (16 байт).
        /// </summary>
        private const int BLOCK_SIZE = 16;
        /// <summary>
        /// Вектор Пи для прямого нелинейного преобразования.<br/>
        /// Также известен под названием SBox.
        /// </summary>
        private readonly byte[] PI =
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
        /// <summary>
        /// Вектор Пи для обратного нелинейного преобразования.<br/>
        /// Также известен под названием SBox.
        /// </summary>
        private readonly byte[] REVESRE_PI =
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
        /// <summary>
        /// Вектор L для линейного преобразования.
        /// </summary>
        private readonly byte[] L = { 1, 148, 32, 133, 16, 194, 192, 1, 251, 1, 192, 194, 16, 133, 32, 148 };
        /// <summary>
        /// Массив для хранения констант.
        /// </summary>
        private byte[][] C = new byte[32][];
        /// <summary>ы
        /// Массив для хранения ключей.
        /// </summary>
        private byte[][] KEY = new byte[10][];
        #endregion

        #region Algorithm mathematics
        /// <summary>
        /// X-преобразование. Исключительное или 2х массивов.
        /// </summary>
        private byte[] XTransformation(byte[] a, byte[] b)
        {
            int i;
            byte[] c = new byte[BLOCK_SIZE];
            for (i = 0; i < BLOCK_SIZE; i++)
                c[i] = (byte)(a[i] ^ b[i]);
            return c;
        }
        /// <summary>
        /// S-преобразование. Нелинейное преобразование из вектора Пи.
        /// </summary>
        private byte[] STransformation(byte[] in_data)
        {
            byte[] out_data = new byte[BLOCK_SIZE];
            for (int i = 0; i < BLOCK_SIZE; i++)
            {
                int data = in_data[i];
                if (data < 0)
                    data = data + 256;
                
                out_data[i] = PI[data];
            }
            return out_data;
        }
        /// <summary>
        /// Обратное S-преобразование.
        /// </summary>
        private byte[] ReverseSTransformation(byte[] in_data)
        {
            byte[] out_data = new byte[in_data.Length];
            for (int i = 0; i < BLOCK_SIZE; i++)
            {
                int data = in_data[i];
                if (data < 0)
                    data = data + 256;

                out_data[i] = REVESRE_PI[data];
            }
            return out_data;
        }

        /// <summary>
        /// Умножение в поле Галуа.
        /// </summary>
        private byte XorMul(byte a, byte b)
        {
            byte c = 0;
            byte hi_bit;
            int i;
            for (i = 0; i < 8; i++)
            {
                if ((b & 1) == 1)
                    c ^= a;

                hi_bit = (byte)(a & 0x80);
                a <<= 1;

                if (hi_bit < 0)
                    a ^= 0xc3; //полином  x^8+x^7+x^6+x+1
                
                b >>= 1;
            }
            return c;
        }

        /// <summary>
        /// R-преобразование. Сдвигает данные и реализует уравнение, представленное для расчета L-функции.
        /// </summary>
        private byte[] RTransformation(byte[] in_data)
        {
            int i;
            byte a_15 = 0;
            byte[] tmp = new byte[16];
            for (i = 15; i >= 0; i--)
            {
                if (i == 0)
                    tmp[15] = in_data[i];
                else
                    tmp[i - 1] = in_data[i];
                a_15 ^= XorMul(in_data[i], L[i]);
            }
            tmp[15] = a_15;
            return tmp;
        }

        /// <summary>
        /// L-преобразование.
        /// </summary>
        private byte[] LTransformation(byte[] in_data)
        {
            int i;
            byte[] out_data = new byte[in_data.Length];
            byte[] tmp = in_data;
            for (i = 0; i < 16; i++)
                tmp = RTransformation(tmp);
            
            out_data = tmp;
            return out_data;
        }

        /// <summary>
        /// Обратное R-преобразование.
        /// </summary>
        private byte[] ReverseRTransformation(byte[] in_data)
        {
            int i;
            byte a_0;
            a_0 = in_data[15];
            byte[] tmp = new byte[16];
            for (i = 1; i < 16; i++)
            {
                tmp[i] = in_data[i - 1];
                a_0 ^= XorMul(tmp[i], L[i]);
            }
            tmp[0] = a_0;
            return tmp;
        }

        /// <summary>
        /// Обратное L-преобразование.
        /// </summary>
        private byte[] ReverseLTransformation(byte[] in_data)
        {
            int i;
            byte[] out_data = new byte[in_data.Length];
            byte[] tmp;
            tmp = in_data;
            for (i = 0; i < 16; i++)
                tmp = ReverseRTransformation(tmp);
            out_data = tmp;
            return out_data;
        }

        private void FTransformation(byte[] input1, byte[] input2, ref byte[] output1, ref byte[] output2, byte[] round_C)
        {
            byte[] state = new byte[16];
            state = XTransformation(input1, round_C);
            state = STransformation(state);
            state = LTransformation(state);
            output1 = XTransformation(state, input2);
            output2 = input1;
        }

        private void KeyGen(byte[] mas_key)
        {
            // Генерация раундовых констант
            byte[][] iterNum = new byte[32][];
            for (int i = 0; i < 32; i++)
            {
                iterNum[i] = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Convert.ToByte(i + 1) };
                this.C[i] = LTransformation(iterNum[i]);
            }

            // Генерация первых 2-х лючей
            byte[] A = new byte[16];
            for (int i = 0; i < 16; i++) A[i] = mas_key[i];
            byte[] B = new byte[16];
            int j = 0;
            for (int i = 16; i < 32; i++)
            {
                B[j] = mas_key[i];
                j++;
            }
            j = 0;
            KEY[0] = B;
            KEY[1] = A;

            byte[] C = new byte[16];
            byte[] D = new byte[16];

            // Генерация остальных ключей
            for (int i = 0; i < 4; i++)
            {
                FTransformation(A, B, ref C, ref D, this.C[0 + 8 * i]);
                FTransformation(C, D, ref A, ref B, this.C[1 + 8 * i]);
                FTransformation(A, B, ref C, ref D, this.C[2 + 8 * i]);
                FTransformation(C, D, ref A, ref B, this.C[3 + 8 * i]);
                FTransformation(A, B, ref C, ref D, this.C[4 + 8 * i]);
                FTransformation(C, D, ref A, ref B, this.C[5 + 8 * i]);
                FTransformation(A, B, ref C, ref D, this.C[6 + 8 * i]);
                FTransformation(C, D, ref A, ref B, this.C[7 + 8 * i]);
                KEY[2 * i + 2] = A;
                KEY[2 * i + 3] = B;
            }


        }
        #endregion

        #region Other
        private string LengthTo32Bytes(string str)
        {
            if (str.Length< 32)
            {
                int diff = 32 - str.Length;
                int j = 0;
                for (int i = str.Length; i< 32; i++)
                {
                    str += str.Substring(j, 1);
                    if (j == str.Length - 1) j = 0;
                    else j++;
                }
                return str;
            }
            else if (str.Length > 32)
                return str = str.Substring(0, 32);
            else
                return str;
        }
        #endregion

        public Kuznechik() { }

        #region Encrypting and decrypting
        /// <summary>
        /// Шифрование сообщения ключом.
        /// </summary>
        public byte[] Encrypt(byte[] arr, byte[] key)
        {
            key = Encoding.Default.GetBytes(LengthTo32Bytes(Encoding.Default.GetString(key)));
            KeyGen(key);
            int NumOfBlocks;  // Определение кол-ва блоков по 16 байт
            int NumberOfNull; // Определение кол-ва недостающих байт последнего блока
            byte[] OriginText = arr;

            byte[] encrText = new byte[0]; // Массив для хранения зашифрованных байтов
            if ((arr.Length % BLOCK_SIZE) == 0)
            {
                NumOfBlocks = arr.Length / BLOCK_SIZE;
                Array.Resize(ref encrText, arr.Length);
            }
            else
            {
                NumOfBlocks = (arr.Length / BLOCK_SIZE) + 1;
                NumberOfNull = NumOfBlocks * BLOCK_SIZE - arr.Length;
                int StartLength = arr.Length;
                Array.Resize(ref OriginText, OriginText.Length + NumberOfNull);
                Array.Resize(ref encrText, OriginText.Length);
                if (NumberOfNull == 1) OriginText[OriginText.Length - 1] = 0x80;
                else
                {
                    for (int i = OriginText.Length - 1; i >= 0; i--)
                    {
                        if (i == OriginText.Length - 1)
                        {
                            OriginText[OriginText.Length - 1] = 0x81;
                        }
                        else if (OriginText[i] != 0)
                        {
                            OriginText[i + 1] = 0x01;
                            break;
                        }
                    }
                }
            }
            
            // Операция зашифровки
            for (int i = 0; i < NumOfBlocks; i++) 
            {
                byte[] block = new byte[BLOCK_SIZE];

                for (int j = 0; j < BLOCK_SIZE; j++)
                    block[j] = OriginText[i * BLOCK_SIZE + j];
                
                for (int j = 0; j < 9; j++)
                {
                    block = XTransformation(block, KEY[j]);
                    block = STransformation(block);
                    block = LTransformation(block);
                }
                
                block = XTransformation(block, KEY[9]);
                for (int j = 0; j < BLOCK_SIZE; j++)
                    encrText[i * BLOCK_SIZE + j] = block[j];
            }
            return encrText;
        }

        /// <summary>
        /// Расшифрование сообщения ключом.
        /// </summary>
        public byte[] Decrypt(byte[] arr, byte[] key)
        {
            key = Encoding.Default.GetBytes(LengthTo32Bytes(Encoding.Default.GetString(key)));
            KeyGen(key);
            int NumOfBlocks = arr.Length / BLOCK_SIZE; // Определение кол-ва блоков по 16 байт
            byte[] OriginText = arr;
            byte[] decrText = new byte[arr.Length]; // Массив для хранения зашифрованных байтов

            for (int i = 0; i < NumOfBlocks; i++)
            {
                byte[] block = new byte[BLOCK_SIZE];
                for (int j = 0; j < BLOCK_SIZE; j++)
                    block[j] = OriginText[i * BLOCK_SIZE + j];
                
                block = XTransformation(block, KEY[9]);
                for (int j = 8; j >= 0; j--)
                {
                    block = ReverseLTransformation(block);
                    block = ReverseRTransformation(block);
                    block = XTransformation(block, KEY[j]);
                }
                
                for (int j = 0; j < BLOCK_SIZE; j++)
                    decrText[i * 16 + j] = block[j];
                
                if (i == NumOfBlocks - 1 && (decrText[decrText.Length - 1] == 0x81 || decrText[decrText.Length - 1] == 0x80))
                {
                    if (decrText[decrText.Length - 1] == 0x81)
                    {
                        int Zeros = 0;
                        for (int j = decrText.Length - 1; j > 0; j--)
                        {
                            if (decrText[j] == 0x81 || decrText[j] == 0x01 || decrText[j] == 0) Zeros++;
                            else break;
                        }
                        Array.Resize(ref decrText, decrText.Length - Zeros);
                    }
                    if (decrText[decrText.Length - 1] == 0x80) Array.Resize(ref decrText, decrText.Length - 1);
                }
            }
            return decrText;
        }

        public string Encrypt(string msg, string key) => Convert.ToHexString(Encrypt(Encoding.Default.GetBytes(msg), Encoding.Default.GetBytes(key)));
        public string Decrypt(string cph, string key) => Convert.ToHexString(Decrypt(Convert.FromHexString(cph), Encoding.Default.GetBytes(key)));
        #endregion
    }
}
