﻿using System.Diagnostics;
using System.Numerics;
using System;
using System.Collections.Generic;

namespace Tools
{
    // https://habr.com/ru/articles/745820/
    class RSA
    {
        private const int BIT_LENGTH = 1024 * 8;
        private const int BITE_LENGTH = BIT_LENGTH / 8;
        private const int TEST_SAMPLES = 5;
        private List<BigInteger> PROBABLE_PRIMES = new List<BigInteger>();

        private byte[] buff = new byte[BITE_LENGTH];
        private Random rng  = new Random();

        public RSA()
        {
            //PROBABLE_PRIMES.Add(BigInteger.Parse());
        }

        public BigInteger GetBigInteger()
        {
            rng.NextBytes(buff);
            return new BigInteger(buff);
        }

        public BigInteger GetBigInteger(int length)
        {
            byte[] arr = new byte[length];
            rng.NextBytes(arr);
            return new BigInteger(arr);
        }

        public BigInteger GetPrimeBigInteger()
        {
            BigInteger i = GetBigInteger();
            Stopwatch sw = Stopwatch.StartNew();
            Stopwatch gsw = Stopwatch.StartNew();
            UInt64 iter = 0;

            while (!IsPrime(i))
            {
                if (iter % 1000 == 0)
                {
                    sw.Stop();
                    Console.WriteLine("[" + (sw.ElapsedMilliseconds / 1000).ToString() + "s] IsPrime() --> " + IsPrime(i) + " --> changing number...");
                    sw.Reset();
                    sw.Start();
                }
                i = GetBigInteger();
                iter++;
            }

            gsw.Stop();
            Console.WriteLine(gsw.ElapsedMilliseconds / 1000 + "s -- total search time");

            return i;
        }

        public bool IsPrime(BigInteger prime) => IsProbablePrime(prime, TEST_SAMPLES);

        /// <summary>
        /// Тест Миллера-Рабина на простоту числа. Производится k раундов проверки числа n на простоту.
        /// </summary>
        public bool IsProbablePrime(BigInteger n, int k)
        {
            // если n == 2 или n == 3 - эти числа простые, возвращаем true
            if (n == 2 || n == 3)
                return true;

            // если n < 2 или n четное - возвращаем false
            if (n < 2 || n % 2 == 0)
                return false;

            // представим n − 1 в виде (2^s)·t, где t нечётно, это можно сделать последовательным делением n - 1 на 2
            BigInteger t = n - 1;

            int s = 0;

            while (t % 2 == 0)
            {
                t /= 2;
                s += 1;
            }

            // повторить k раз
            for (int i = 0; i < k; i++)
            {
                // выберем случайное целое число a в отрезке [2, n − 2]
                

                byte[] _a = new byte[n.ToByteArray().LongLength];

                BigInteger a;

                do
                {
                    rng.NextBytes(_a);
                    a = new BigInteger(_a);
                }
                while (a < 2 || a >= n - 2);

                // x ← a^t mod n, вычислим с помощью возведения в степень по модулю
                BigInteger x = BigInteger.ModPow(a, t, n);

                // если x == 1 или x == n − 1, то перейти на следующую итерацию цикла
                if (x == 1 || x == n - 1)
                    continue;

                // повторить s − 1 раз
                for (int r = 1; r < s; r++)
                {
                    // x ← x^2 mod n
                    x = BigInteger.ModPow(x, 2, n);

                    // если x == 1, то вернуть "составное"
                    if (x == 1)
                        return false;

                    // если x == n − 1, то перейти на следующую итерацию внешнего цикла
                    if (x == n - 1)
                        break;
                }

                if (x != n - 1)
                    return false;
            }

            // вернуть "вероятно простое"
            return true;
        }
    }
}
