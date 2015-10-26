using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PenYourPrayer.Controllers
{
    public class MyPasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return password + "hahaha";
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if(hashedPassword.Equals(providedPassword))
                return PasswordVerificationResult.Success;
            else return PasswordVerificationResult.Failed;
        }
    
    }
}