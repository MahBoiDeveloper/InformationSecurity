using System.IO;
using System.Text;
using System.Text.Json;
using System.Security.Principal;
using System.Security.Cryptography;
using System.Linq;

namespace InformationSecurity
{
    static class Authentication
    {
        public static readonly JsonDocument doc = JsonDocument.Parse(File.ReadAllText(ProgramConstants.USERS_JSON));
        public static readonly Random rng = new Random();

        static Authentication() {}

        public static bool CheckLocalAccountForLogin(string login)
        {
            foreach (var item in doc.RootElement.GetProperty("userdata").EnumerateArray())
                if (item.GetProperty("allowed_local_account").ToString() == WindowsIdentity.GetCurrent().Name &&
                    item.GetProperty("login").ToString() == login)
                    return true;
            
            return false;
        }

        public static bool CheckCodeForLogin(string login, string code)
        {
            foreach (var item in doc.RootElement.GetProperty("userdata").EnumerateArray())
                if (item.GetProperty("login").ToString() == login)
                    foreach (var elem in item.GetProperty("codes").EnumerateArray())
                        if (elem.ToString() == code)
                            return true;

            return false;
        }

        public static bool CheckLoginWithPassword(string login, string password)
        {
            foreach (var userdata in doc.RootElement.GetProperty("userdata").EnumerateArray())
            {
                if (userdata.GetProperty("login").ToString() != login)
                    continue;

                var salts = doc.RootElement.GetProperty("salt").EnumerateArray();
                int i = 0;
                int stop = rng.Next() % salts.Count();
                foreach (var salt in salts)
                {
                    if (stop != i++)
                        continue;

                    if (GetSalted(userdata.GetProperty("password").ToString(), salt.ToString()) == GetSalted(password, salt.ToString()))
                        return true;
                }
                    
            }
            
            return false;
        }

        public static bool CheckForSpecialSymbols(string src)
        {
            if (src.Contains('\'') || src.Contains('\"') || 
                src.Contains('\\') || src.Contains('\"') || 
                src.Contains('/') || src.Contains('|')) 
                return true;

            return false;
        }

        // SHA384(SHA256(src) + salt) -> SHA512 -> MD5
        private static string GetSalted(string password, string salt)
        {
            MD5 md5 = MD5.Create();
            SHA256 sha256 = SHA256.Create();
            SHA384 sha384 = SHA384.Create();
            SHA512 sha512 = SHA512.Create();

            string hash_login;
            string hash_password;
            string hash;

            hash_login = Convert.ToHexString(sha256.ComputeHash(Encoding.Default.GetBytes(password)));
            hash_password = Convert.ToHexString(sha384.ComputeHash(Encoding.Default.GetBytes(hash_login + salt)));

            hash = Convert.ToHexString(md5.ComputeHash(sha512.ComputeHash(Convert.FromHexString(hash_login + hash_password))));

            return hash;
        }
    }
}
