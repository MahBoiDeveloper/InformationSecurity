using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

using Tools;
using System.Text;
class Program
{
    static void Main(string[] args)
    {
        RSATest();
        //StreebogTest();
        //TryGenerateProbablePrimesViaGithub();
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
        string msg = "Hello world!";
        Console.WriteLine("Original message: " + msg);
        var cipher = rsa.Encrypt("Hello world!");
        var msgfromcipher = rsa.Decrypt(cipher);
        Console.WriteLine("Deciphered message: " + msgfromcipher);

        if (msg != msgfromcipher)
        {
            Console.WriteLine("Messages aren't equal!");
            rsa.DebugPrint();
        }
    }
}
