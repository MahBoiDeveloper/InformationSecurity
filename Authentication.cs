using System.IO;
using System.Text;
using System.Text.Json;
using System.Security.Principal;
using System.Security.Cryptography;
using System.Linq;
using System.Windows;

namespace InformationSecurity
{
    static class Authentication
    {
        public class User
        {
            public string login { get; set; } = string.Empty;
            public string password { get; set; } = string.Empty;
            public string allowed_local_account { get; set; } = string.Empty;
            public string code { get; set; } = string.Empty;
        }

        private static readonly Random       rng = new Random();
        private static readonly JsonDocument doc = JsonDocument.Parse(File.ReadAllText(ProgramConstants.USERS_JSON));
        public  static readonly List<string> Salts = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(ProgramConstants.SALT_JSON));
        public  static readonly List<User>   Users = JsonSerializer.Deserialize<List<User>>(File.ReadAllText(ProgramConstants.USERS_JSON));

        static Authentication() 
        {
            return;
        }

        public static bool CheckLocalAccountForLogin(string login)
        {
            foreach (var user in Users)
                if (user.allowed_local_account == WindowsIdentity.GetCurrent().Name &&
                    user.login == login)
                    return true;
            
            return false;
        }

        public static bool CheckCodeForLogin(string login, string code)
        {
            foreach (var user in Users)
                if (user.login == login)
                    if (user.code == code)
                        return true;

            return false;
        }

        public static bool CheckLoginWithPassword(string login, string password)
        {
            int i = rng.Next() % Salts.Count();

            foreach (var user in Users)
            {
                if (user.login != login)
                    continue;

                if (GetSalted(user.password, Salts[i]) == GetSalted(password, Salts[i]))
                    return true;
            }
            
            return false;
        }

        public static bool CheckForSpecialSymbols(string src) => src.Contains('\'') || src.Contains('\"') ||
                                                                 src.Contains('\\') || src.Contains('\"') ||
                                                                 src.Contains('/')  || src.Contains('|') ? true : false;

        // SHA384(SHA256(src) + salt) -> SHA512 -> Streebog -> MD5
        private static string GetSalted(string password, string salt)
        {
            MD5 md5 = MD5.Create();
            SHA256 sha256 = SHA256.Create();
            SHA384 sha384 = SHA384.Create();
            SHA512 sha512 = SHA512.Create();
            Streebog stb = new Streebog();

            string hash_login;
            string hash_password;
            string hash;

            hash_login = Convert.ToHexString(sha256.ComputeHash(Encoding.Default.GetBytes(password)));
            hash_password = Convert.ToHexString(sha384.ComputeHash(Encoding.Default.GetBytes(hash_login + salt)));

            hash = Convert.ToHexString(
                                       md5.ComputeHash(
                                       stb.ComputeHash(
                                       sha512.ComputeHash(Convert.FromHexString(hash_login + hash_password)))));

            return hash;
        }
    }
}
