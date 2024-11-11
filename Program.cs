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

        Console.WriteLine("Hashing 256-bit");
        Console.WriteLine("Original msg: " + Convert.ToHexString(message));
        Console.WriteLine("Hash: " + sbg.GetHash(message));
        Console.WriteLine("Original msg: " + tmp);
        Console.WriteLine("Hash: " + sbg.GetHash(tmp));
        Console.WriteLine();
        Console.WriteLine("Hashing 512-bit");
        Console.WriteLine("Original msg: " + Convert.ToHexString(message));
        Console.WriteLine("Hash: " + sbg.GetHash512(message));
        Console.WriteLine("Original msg: " + tmp);
        Console.WriteLine("Hash: " + sbg.GetHash512(tmp));
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
        Kuznechik kzn = new Kuznechik();

        byte[] msg =
        //Encoding.Default.GetBytes("Привет мир!");
        //Convert.FromHexString("1122334455667700ffeeddccbbaa9988");
        Encoding.Default.GetBytes("Бу! Испугался? Не бойся, я друг, я тебя не обижу. Иди сюда, иди ко мне, сядь рядом со мной, посмотри мне в глаза. Ты видишь меня? Я тоже тебя вижу. Давай смотреть друг на друга до тех пор, пока наши глаза не устанут. Ты не хочешь? Почему? Что-то не так?");

        Console.WriteLine("msg.len = " + msg.Length);

        byte[] cip = kzn.Encrypt(msg);
        Console.WriteLine("Original: " + Encoding.Default.GetString(msg));
        Console.WriteLine("Cipher:   " + Convert.ToHexString(cip));
        Console.WriteLine("Decipher: " + Encoding.Default.GetString(kzn.Decrypt(cip)));
    }
}
