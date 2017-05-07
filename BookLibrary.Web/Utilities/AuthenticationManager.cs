namespace BookLibrary.Web.Utilities
{
    using BookLibrary.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using BookLibrary.Data;

    public class AuthManager
    {
        static User currentUser;
        private static BookLibraryContext context = new BookLibraryContext();

        public static bool IsUserExisting(string username, string password)
        {
            return context.Users.Any(u => u.Username == username && u.Password == password);
        }

        public static void SetCurrentUser(string username, string password)
        {
            if (IsUserExisting(username, password))
            {
                currentUser = GetLoggedUser(username);
            }
            else
            {
                throw new InvalidOperationException("error setting user");
            }
        }

        public static User GetAuthenticated()
        {
            return currentUser;
        }

        public static User GetLoggedUser(string username)
        {
            return context.Users.Where(u => u.Username == username).FirstOrDefault();
        }

        public static void LogoutUser()
        {
            currentUser = null;
        }

        public static bool IsUserLoggedIn()
        {
            if(GetAuthenticated() == null)
            {
                return false;
            }
            return true;
        }
    }
}