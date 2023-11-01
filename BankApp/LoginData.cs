using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    public struct LoginData
    {
        public string Login;
        public string Role;
        public int Icon;
        public int? ClientID;

        public LoginData(string Login, string Role, int Icon, int? ClientID)
        {
            this.Login = Login;
            this.Role = Role;
            this.Icon = Icon;
            this.ClientID = ClientID;
        }
    }
}
