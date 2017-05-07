namespace BookLibrary.Data
{
    using BookLibrary.Models;
    using System.Data.Entity;
    using System.Web;
    public class MyInitiliazer : DropCreateDatabaseAlways<BookLibraryContext>
    {
        protected override void Seed(BookLibraryContext context)
        {
            var admin = new User()
            {
                Email = "admin@abv.bg",
                Name = "Admin",
                Password = "admin",
                Username = "admin",
                Role = (Role)2,
                RegisterProfilePicture = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("../Content/myProfileIcon.png"))
            };

            context.Users.Add(admin);
            base.Seed(context);
        }
    }
}