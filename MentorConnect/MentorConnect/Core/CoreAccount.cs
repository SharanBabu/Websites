using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MentorConnect.Repository;
using MentorConnect.Models;

namespace MentorConnect.Core
{
    public class CoreAccount
    {
        AccountRepository userRep = new AccountRepository();
        public string IsAuthenticated(string username, string password, bool isMentor)
        {
            string fname = "";
            User user = userRep.GetUserByUserNamePassword(username,password, isMentor);
            if (user != null)
                fname = user.fname;
            return fname;
        }

        public bool IsSuccessfulRegisteration(RegisterModel regMod)
        {
            bool isSuccess = false;
            userRep.RegisterNewUser(regMod);
            isSuccess = true;
            return isSuccess;
        }
    }
}