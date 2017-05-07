
namespace BookLibrary.Web.Controllers
{
    using BookLibrary.Models;
    using BookLibrary.Web.Utilities;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using BookLibrary.Data;

    public class HomeController : Controller
    {
        BookLibraryContext db = new BookLibraryContext();

        public ActionResult Index()
        {
            User currentUser = AuthManager.GetAuthenticated();
            if (currentUser != null)
            {
                List<Book> list = new List<Book>();
                var booksToShow = db.Users.Find(currentUser.Id).Following.Select(f => f.Books);
                foreach (var userPictures in booksToShow)
                {
                    foreach (var picture in userPictures)
                    {
                        list.Add(picture);
                    }
                }
                list = list.OrderByDescending(p => p.UploadDate).ToList();
                return View(list);
            }
            else return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}