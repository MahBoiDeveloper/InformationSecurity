using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationSecurity
{
    public interface ICryptoAlgorithm
    {
        string Encrypt(string message);
        string Decrypt(string cipher);

        byte[] Encrypt(byte[] message);
        byte[] Decrypt(byte[] cipher);
    }
}
