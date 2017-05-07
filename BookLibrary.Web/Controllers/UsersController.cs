namespace BookLibrary.Web.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using BookLibrary.Data;
    using BookLibrary.Models;
    using BookLibrary.Web.Validations;
    using BookLibrary.Web.Utilities;

    public class UsersController : Controller
    {
        private BookLibraryContext db = new BookLibraryContext();

        // GET: Users
        public ActionResult Index()
        {
            if (!AuthManager.IsUserLoggedIn() || !UserUtilities.IsCurrentUserAdmin())
            {
                return View("NoAccess");
            }
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            var authentication = AuthManager.GetAuthenticated();
            if (authentication != null)
            {
                return RedirectToAction("Details/" + authentication.Id, "Users");
            }

            if (ViewBag.ShowError == null)
                ViewBag.ShowError = false;

            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Username,Password,Email,RepeatPassword,Biography")] User user, HttpPostedFileBase profilePictureFile)
        {
            if (!UserValidations.ValidateEmail(user.Email))
            {
                ViewBag.Error = "Your email is invalid. Please enter a valid one.";
                ViewBag.ShowError = true;
                return View();
            }

            if (UserUtilities.IsEmailTaken(user.Email, db))
            {
                ViewBag.Error = "This email is already taken. Please register with another one.";
                ViewBag.ShowError = true;
                return View();
            }

            if (!UserValidations.ValidateUsername(user.Username))
            {
                ViewBag.Error = "Your username is invalid. It should be between 3 and 50 characters long.";
                ViewBag.ShowError = true;
                return View();
            }

            if (UserUtilities.IsUserExisting(user.Username, db))
            {
                ViewBag.Error = "This username is already taken. Please register with another one.";
                ViewBag.ShowError = true;
                return View();
            }

            if (!UserValidations.ValidatePassword(user.Password))
            {
                ViewBag.Error = "Your password is invalid. It should be at least 8 characters long, contain at least one small and one big letter and one digit.";
                ViewBag.ShowError = true;
                return View();
            }

            if (!UserValidations.ValidateRepeatedPassword(user.Password, user.RepeatPassword))
            {
                ViewBag.Error = "Your passwords do not match.";
                ViewBag.ShowError = true;
                return View();
            }

            if (!UserValidations.ValidateProfilePicture(profilePictureFile))
            {
                ViewBag.Error = "You have not chosen a profile picture.";
                ViewBag.ShowError = true;
                return View();
            }

            if (ModelState.IsValid)
            {
                user.RegisterProfilePicture = PictureUtilities.PictureToByteArray(profilePictureFile);
                user.Role = (Role)0;
                db.Users.Add(user);
                db.SaveChanges();
                AuthManager.SetCurrentUser(user.Username, user.Password);
                return RedirectToAction("Details/" + AuthManager.GetAuthenticated().Id, "Users");
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (ViewBag.ShowError == null)
                ViewBag.ShowError = false;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (!UserUtilities.IsCurrentUserIdEqualTo(user.Id))
            {
                return View("NoAccess");
            }
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Username,Password,Email,RepeatPassword")] User user, HttpPostedFileBase profilePictureFile)
        {
            if (!UserValidations.ValidateEmail(user.Email))
            {
                ViewBag.Error = "Your email is invalid. Please enter a valid one.";
                ViewBag.ShowError = true;
                return View();
                //TODO: Add notification
            }

            if (!UserValidations.ValidateUsername(user.Username))
            {
                ViewBag.Error = "Your username is invalid. It should be between 3 and 50 characters long.";
                ViewBag.ShowError = true;
                return View();
                //TODO: Add notification
            }

            if (!UserValidations.ValidatePassword(user.Password))
            {
                ViewBag.Error = "Your password is invalid. It should be at least 8 characters long, contain at least one small and one big letter and one digit.";
                ViewBag.ShowError = true;
                return View();
                //TODO: Add notification
            }

            if (!UserValidations.ValidateRepeatedPassword(user.Password, user.RepeatPassword))
            {
                ViewBag.Error = "Your passwords do not match.";
                ViewBag.ShowError = true;
                return View();
                //TODO: Add notification
            }

            if (!UserValidations.ValidateProfilePicture(profilePictureFile))
            {
                ViewBag.Error = "You have not chosen a profile picture.";
                ViewBag.ShowError = true;
                return View();
                //TODO: Add notification
            }

            if (ModelState.IsValid)
            {
                user.RegisterProfilePicture = PictureUtilities.PictureToByteArray(profilePictureFile);
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details/" + AuthManager.GetAuthenticated().Id, "Users");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            if (!AuthManager.IsUserLoggedIn() || !UserUtilities.IsCurrentUserAdmin())
            {
                return View("NoAccess");
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            NotificationsController.RemoveUserNotifications(id);
            foreach (var follower in user.Followers.ToList())
            {
                user.Followers.Remove(follower);
                db.SaveChanges();
            }
            foreach (var following in user.Following.ToList())
            {
                user.Following.Remove(following);
                db.SaveChanges();
            }
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Users/Login
        public ActionResult Login()
        {
            var authentication = AuthManager.GetAuthenticated();
            if (authentication != null)
            {
                return RedirectToAction("Details/" + authentication.Id, "Users");
            }

            if (ViewBag.ShowError == null)
                ViewBag.ShowError = false;

            return View();
        }


        // POST: Users/Login
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            var user = new User();
            if (ModelState.IsValid)
            {
                if (AuthManager.IsUserExisting(username, password))
                {
                    AuthManager.SetCurrentUser(username, password);
                    user = AuthManager.GetAuthenticated();
                    return RedirectToAction("Details/" + AuthManager.GetLoggedUser(username).Id);
                }
                else
                {
                    ViewBag.ErrorMessage = "The username or the password is incorrect.";
                    ViewBag.ShowError = true;
                    return View();
                }
            }

            return View(user);
        }

        [ActionName("Logout")]
        public ActionResult LogoutUser()
        {
            AuthManager.LogoutUser();
            return RedirectToAction("Login");
        }

        [ActionName("Follow")]
        public ActionResult Follow(int id, int loggedId)
        {
            User userToFollow = db.Users.Find(id);
            User userFollowing = db.Users.Find(loggedId);
            userToFollow.Followers.Add(userFollowing);
            NotificationsController.AddFollowNotification(loggedId, id);
            db.SaveChanges();
            return RedirectToAction($"Details/{id}");
        }

        [ActionName("Unfollow")]
        public ActionResult Unfollow(int id, int loggedId)
        {
            User userToUnfollow = db.Users.Find(id);
            User userUnfollowing = db.Users.Find(loggedId);
            userToUnfollow.Followers.Remove(userUnfollowing);
            NotificationsController.RemoveFollowNotification(loggedId, id);
            db.SaveChanges();

            return RedirectToAction($"Details/{id}");
        }
    }
}
