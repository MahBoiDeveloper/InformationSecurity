using System;
using System.Collections.Generic;
using System.Linq;

using Tools;
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
        for (int i = 0; i < 8; i++) tmp.Add(i);
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
        Console.WriteLine(N.GetBitLength());

    }
}
