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
    class Authentication
    {
        public readonly JsonDocument doc = JsonDocument.Parse(File.ReadAllText(ProgramConstants.USERS_JSON));
        
        public Authentication() {}

        public bool CheckLocalAccountForLogin(string login)
        {
            foreach (var item in doc.RootElement.GetProperty("userdata").EnumerateArray())
            {
                if (item.GetProperty("allowed_local_account").ToString() == WindowsIdentity.GetCurrent().Name &&
                    item.GetProperty("login").ToString() == login)
                    return true;
            }
            
            return false;
        }
    }
}
