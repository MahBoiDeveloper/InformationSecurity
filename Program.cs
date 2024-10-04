using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

using Tools;
using System.Xml.Linq;
class Program
{
    static void Main(string[] args)
    {
        //RSATest();
        //StreebogTest();
        TryGenerateProbablePrimesViaGithub();
    }
    static void TryGenerateProbablePrimesViaGithub()
    {
        RSA rsa = new RSA();
        List<int> tmp = new List<int>();
        List<string> list = new List<string>();
        for (int i = 0; i < 6; i++) tmp.Add(i);
        tmp.AsParallel().ForAll(x => list.Add(rsa.GetPrimeBigInteger().ToString()));
        list.ForEach(x => Console.WriteLine(x));
    }
    static void StreebogTest()
    {
        string tmp = "Hello World!";
        byte[] message =
        {
            0x32,0x31,0x30,0x39,0x38,0x37,0x36,0x35,0x34,0x33,0x32,0x31,0x30,0x39,0x38,0x37,
            0x36,0x35,0x34,0x33,0x32,0x31,0x30,0x39,0x38,0x37,0x36,0x35,0x34,0x33,0x32,0x31,
            0x30,0x39,0x38,0x37,0x36,0x35,0x34,0x33,0x32,0x31,0x30,0x39,0x38,0x37,0x36,0x35,
            0x34,0x33,0x32,0x31,0x30,0x39,0x38,0x37,0x36,0x35,0x34,0x33,0x32,0x31,0x30
        };

        Console.WriteLine(new Streebog().GetHash(message));
        Console.WriteLine(new Streebog().GetHash(tmp));
    }
    static void RSATest() 
    {
        RSA rsa = new RSA();
        var tpl = rsa.GetPrimePair();
        var N = tpl.Item1 * tpl.Item2;
        var fi_N = (tpl.Item1 - 1) * (tpl.Item2 - 1);
        var e = 131073;
        var temp = rsa.EuclidAlgorithm(fi_N, e);
        var x = temp.Item1 < temp.Item2 ? temp.Item1 : temp.Item2;
        var d = fi_N - x;
        //Console.WriteLine(N.GetBitLength());
        //Console.WriteLine(fi_N.GetBitLength());
        Console.WriteLine(e);
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
