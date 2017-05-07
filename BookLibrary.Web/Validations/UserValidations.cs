using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace BookLibrary.Web.Validations
{
    public class UserValidations
    {
        private static Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        private static Regex passRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{5,30}$");

        public static bool ValidateEmail(string email)
        {
            if (email != null && emailRegex.IsMatch(email))
                return true;
            else
                return false;
        }

        public static bool ValidateUsername(string username)
        {
            if (username == null || username.Length < 3 || username.Length > 50)
                return false;
            else
                return true;
        }

        public static bool ValidatePassword(string pass)
        {
            if (pass != null && passRegex.IsMatch(pass))
                return true;
            else
                return false;
        }

        public static bool ValidateRepeatedPassword(string pass, string repeatedPass)
        {
            if (pass == repeatedPass)
                return true;
            else
                return false;
        }

        public static bool ValidateProfilePicture(HttpPostedFileBase profPic)
        {
            if (profPic == null)
                return false;
            else
                return true;
        }
    }
}