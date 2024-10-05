using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

using Tools;
class Program
{
    static void Main(string[] args)
    {
        //Console.WriteLine(rsa.GetPrimeBigInteger());

        //RSATest();
        //StreebogTest();
        TryGenerateProbablePrimesViaGithub();
    }
    static void TryGenerateProbablePrimesViaGithub()
    {
        RSA rsa = new RSA();
        List<int> tmp = new List<int>();
        List<string> list = new List<string>();
        for (int i = 0; i < 4; i++) tmp.Add(i);
        tmp.AsParallel().ForAll(x => list.Add(rsa.GetPrimeBigInteger().ToString()));
        list.ForEach(x => Console.WriteLine(x));
    }
    static void StreebogTest()
    {
        Streebog sbg = new Streebog();
        string tmp = "Hello World!";
        byte[] message =
        {
            0x32,0x31,0x30,0x39,0x38,0x37,0x36,0x35,0x34,0x33,0x32,0x31,0x30,0x39,0x38,0x37,
            0x36,0x35,0x34,0x33,0x32,0x31,0x30,0x39,0x38,0x37,0x36,0x35,0x34,0x33,0x32,0x31,
            0x30,0x39,0x38,0x37,0x36,0x35,0x34,0x33,0x32,0x31,0x30,0x39,0x38,0x37,0x36,0x35,
            0x34,0x33,0x32,0x31,0x30,0x39,0x38,0x37,0x36,0x35,0x34,0x33,0x32,0x31,0x30
        };

        Console.WriteLine(sbg.GetHash(message));
        Console.WriteLine(sbg.GetHash(tmp));
    }
    static void RSATest()
    {
        RSA rsa = new RSA();
        var tpl = rsa.GetPrimePair();
        var N = tpl.Item1 * tpl.Item2;
        var fi_N = (tpl.Item1 - 1) * (tpl.Item2 - 1);
        var e = 131101;
        var x = rsa.EuclidAlgorithm(fi_N, e);
        var d = fi_N - x;
        //Console.WriteLine(N.GetBitLength());
        //Console.WriteLine(fi_N.GetBitLength());
        Console.WriteLine(d);
        Console.WriteLine(d.GetBitLength());
    }
    static BigInteger FixBitLengthForPrimeBigInteger(BigInteger bigint)
    {
        RSA rsa = new RSA();
        Int32 n = Convert.ToInt32(1024 * 8 - Convert.ToInt32(bigint.GetBitLength()));
        var qwe = bigint << n;
        qwe += BigInteger.One;

        if (bigint.GetBitLength() != 1024 * 8)
        {
            Console.WriteLine(bigint.GetBitLength());
            Console.WriteLine(qwe.GetBitLength());
            //Console.WriteLine("i = 0, bits = " + qwe.GetBitLength() + ", " + rsa.IsProbablePrime(qwe, 10));
            for (int i = 1; i < Convert.ToUInt32(Math.Pow(10, n)); i++)
            {
                qwe += 2;
                bool res = rsa.IsProbablePrime(qwe, 10);
                //Console.WriteLine("i = " + i + ", bits = " + qwe.GetBitLength() + ", " + res);
                if (res) Console.WriteLine(qwe);
            }
        }
        return qwe;
    }
    static BigInteger Power(BigInteger value, BigInteger pow)
    {
        BigInteger result = 1;
        while (pow > 0)
        {
            if (pow % 2 == 1)
                result *= value;
            value *= value;
            pow /= 2;
        }
        return result;
    }
}
