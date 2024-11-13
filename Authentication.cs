using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Security.Principal;

namespace InformationSecurity
{
    static class Authentication
    {
        public static readonly JsonDocument doc = JsonDocument.Parse(File.ReadAllText(ProgramConstants.USERS_JSON));
        
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
    }
}
