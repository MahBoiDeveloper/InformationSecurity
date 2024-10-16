using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

using InformationSecurity;
using System.Text;
class Program
{
    static void Main(string[] args)
    {
        //RSATest();
        //StreebogTest();
        KuznechikTest();
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
        var cipher = rsa.Encrypt(msg);
        var msgfromcipher = rsa.Decrypt(cipher);
        Console.WriteLine("Deciphered message: " + msgfromcipher);
        Console.WriteLine("Cipher: " +  cipher);

        if (msg != msgfromcipher)
        {
            Console.WriteLine("Messages aren't equal!");
            rsa.DebugPrint();
        }
    }
    static void KuznechikTest()
    {
        byte[] key = Convert.FromHexString("8899aabbccddeeff0011223344556677fedcba98765432100123456789abcdef");
        byte[] msg = Convert.FromHexString("1122334455667700ffeeddccbbaa9988");
        
        Kuznechik kzn = new Kuznechik();
        Console.WriteLine(Convert.ToHexString(kzn.Encrypt(msg, key)));
        Console.WriteLine(Convert.ToHexString(kzn.Decrypt(kzn.Encrypt(msg, key), key)));
    }
}
