
namespace BookLibrary.Web.Controllers
{
    using BookLibrary.Models.ViewModels;
    using System.Linq;
    using System.Web.Mvc;
    using BookLibrary.Data;

    public class SearchController : Controller
    {
        BookLibraryContext db = new BookLibraryContext();

        // GET: Search
        [HttpPost]
        public ActionResult Index()
        {
            string searchedWord;
            searchedWord = Request.Form["searchEngine"];
            if (searchedWord == null || searchedWord == "")
            {
                SearchViewModel empty = new SearchViewModel()
                {
                    IsAnythingFound = false
                };
                return View(empty);
            }
            else
            {
                SearchViewModel model = new SearchViewModel()
                {
                    Tags = db.Tags.Where(t => t.Name.Contains(searchedWord)).ToList(),
                    Users = db.Users.Where(u => u.Username.Contains(searchedWord)).ToList(),
                    Books = db.Books.Where(b => b.Title.Contains(searchedWord)).ToList(),
                    IsAnythingFound = true
                };
                if (model.Tags.Count == 0 && model.Users.Count == 0 && model.Books.Count == 0)
                    model.IsAnythingFound = false;
                return View(model);
            }
        }
    }
}