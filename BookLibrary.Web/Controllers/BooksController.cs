using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;

namespace BookLibrary.Web
{
    using System.Web;
    using System.Web.Mvc;
    using BookLibrary.Models;
    using BookLibrary.Web.Utilities;
    using BookLibrary.Models.ViewModels;
    using BookLibrary.Web.Controllers;
    using BookLibrary.Data;

    public class BooksController : Controller
    {
        private BookLibraryContext db = new BookLibraryContext();

        // GET: Books
        public ActionResult Index()
        {
            if (!UserUtilities.IsCurrentUserAdmin())
            {
                return View("NoAccess");
            }

            var books = db.Books.Include(b => b.User);
            return View(books.ToList());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            book.Reads += 1;
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
            if (book == null)
            {
                return HttpNotFound();
            }
            BookDetailsViewModel bookToView = new BookDetailsViewModel()
            {
                Id = book.Id,
                Content = book.Content,
                Tags = book.Tags,
                UserId = book.UserId,
                UploadDate = book.UploadDate,
                Title = book.Title,
                User = book.User,
                Reads = book.Reads,
                Comments = book.Comments
            };

            return View(bookToView);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            if (!AuthManager.IsUserLoggedIn())
            {
                return View("NoAccess");
            }
            if (ViewBag.ShowError == null)
                ViewBag.ShowError = false;
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateBookViewModel book)
        {
            if (book.Title == null)
            {
                ViewBag.Error = "Your book must have a title.";
                ViewBag.ShowError = true;
                return View();
            }
            if (book.Content == null)
            {
                ViewBag.Error = "Your book must have content.";
                ViewBag.ShowError = true;
                return View();
            }
            if (book.CoverPictureFile == null)
            {
                ViewBag.Error = "You have not chosen a cover picture.";
                ViewBag.ShowError = true;
                return View();
            }
            if (book.TagsString == null)
            {
                ViewBag.Error = "You have not selected a tag.";
                ViewBag.ShowError = true;
                return View();
            }
            if (ModelState.IsValid)
            {
                var tagsList = book.TagsString.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                var currentUser = AuthManager.GetAuthenticated();

                var bookEntity = new Book()
                {
                    Content = book.Content,
                    Title = book.Title,
                    UserId = currentUser.Id,
                    Cover = Utilities.PictureUtilities.PictureToByteArray(book.CoverPictureFile)
                };
                db.Users.Find(currentUser.Id).Role = (Role)1;

                bookEntity = TagsController.Create(tagsList, bookEntity, db);
                db.Books.Add(bookEntity);
                db.SaveChanges();
                NotificationsController.AddBookNotification(bookEntity.Id, bookEntity.UserId, currentUser.Followers.ToList());

                return RedirectToAction("Details/" + currentUser.Id, "Users");
            }

            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            if (!(UserUtilities.IsCurrentUserIdEqualTo(book.UserId) || UserUtilities.IsCurrentUserAdmin()))
            {
                return View("NoAccess");
            }
            if (ViewBag.ShowError == null)
                ViewBag.ShowError = false;
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", book.UserId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content,UserId,UploadDate,Cover")] Book book, HttpPostedFileBase coverFile)
        {
            if (book.Title == null)
            {
                ViewBag.Error = "Your book must have a title.";
                ViewBag.ShowError = true;
                return View();
            }
            if (book.Content == null)
            {
                ViewBag.Error = "Your book must have content.";
                ViewBag.ShowError = true;
                return View();
            }
            if (ModelState.IsValid)
            {
                var currentUser = AuthManager.GetAuthenticated();
                book.UserId = currentUser.Id;
                if (coverFile == null)
                {
                    book.Cover = book.Cover;
                }
                else
                {
                    book.Cover = PictureUtilities.PictureToByteArray(coverFile);
                }
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details/" + book.Id);
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", book.UserId);
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            if (!(UserUtilities.IsCurrentUserIdEqualTo(book.UserId) || UserUtilities.IsCurrentUserAdmin()))
            {
                return View("NoAccess");
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            NotificationsController.RemoveBookNotification(id);
            db.Books.Remove(book);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Books/MostPopular
        public ActionResult MostPopular()
        {
            var books = db.Books.OrderByDescending(b => b.Reads).ToList();
            return View(books);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
