namespace BookLibrary.Web.Utilities
{
    using BookLibrary.Models;
    using System.Linq;
    using BookLibrary.Data;

    public static class UserUtilities
    {
        public static bool IsUserExisting(string username, BookLibraryContext db)
        {
            return db.Users.Any(u => u.Username == username);
        }

        public static bool IsEmailTaken(string emailAddress, BookLibraryContext db)
        {
            return db.Users.Any(u => u.Email == emailAddress);
        }

        public static bool IsCurrentUserAdmin()
        {
            if (!AuthManager.IsUserLoggedIn())
            {
                return false;
            }
            else if (AuthManager.GetAuthenticated().Role == (Role)2)
            {                
                return true;
            }
            return false;
        }

        public static string GetUserRole(User model)
        {            
            Role enumDisplayStatus = (Role)model.Role;
            return enumDisplayStatus.ToString();
        }

        public static bool IsCurrentUserIdEqualTo(int authorId)
        {
            if (!AuthManager.IsUserLoggedIn())
            {
                return false;
            }
            return authorId == AuthManager.GetAuthenticated().Id;
        }
    }
}