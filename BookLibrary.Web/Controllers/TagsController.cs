namespace BookLibrary.Web.Controllers
{
    using BookLibrary.Models;
    using BookLibrary.Web.Utilities;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using BookLibrary.Data;

    public class TagsController : Controller
    {
        BookLibraryContext db = new BookLibraryContext();

        // GET: Tags
        public ActionResult Index()
        {
            return View();
        }

        // GET: Tags/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // GET: Tags/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public static Book Create(string[] arrayOfTagStrings, Book currentBook, BookLibraryContext context)
        {
            List<Tag> tagsList = new List<Tag>();
            foreach (var tag in arrayOfTagStrings)
            {
                if (!TagUtilities.IsTagExisting(tag, context))
                {
                    var currentTag = new Tag()
                    {
                        Name = tag
                    };

                    tagsList.Add(currentTag);
                    context.Tags.Add(currentTag);
                }
                else
                {
                    var currentTag = context.Tags.Where(t => t.Name == tag).FirstOrDefault();
                    tagsList.Add(currentTag);
                }
            }
            context.SaveChanges();

            foreach (Tag tag in tagsList)
            {
                currentBook.Tags.Add(tag);
            }
            return currentBook;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [ActionName("DisplayTag")]
        public ActionResult DisplayTag(int id)
        {
            Tag tag = db.Tags.Find(id);
            return RedirectToAction($"Details/{id}", "Tags");
        }
    }
}