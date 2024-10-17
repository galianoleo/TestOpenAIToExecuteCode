using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace TestOpenAIToExecuteCode
{
    public class UserService
    {
        [KernelFunction("user_info")]
        [Description("esta función debe ejecutarse cada vez que el usuario se presente e ingrese información personal")]
        public UserInfo GetUserInfo([Description("el nombre del usuario")] string firstName,
            [Description("el apellido del usuario")] string lastName,
            [Description("el mail del usuario")] string email,
            [Description("la fecha de cumple años del usuario en formato dd/MM/yyyy")] string birthday)
        {

            UserInfo userInfo = new UserInfo()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Birthday = DateTime.ParseExact(birthday, "dd/MM/yyyy", null)
            };
            return userInfo;
        }
    }
}
