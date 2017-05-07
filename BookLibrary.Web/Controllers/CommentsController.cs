namespace BookLibrary.Web.Controllers
{
    using BookLibrary.Data;
    using BookLibrary.Models;
    using BookLibrary.Web.Utilities;
    using System.Web.Mvc;

    public class CommentsController : Controller
    {
        private static BookLibraryContext db = new BookLibraryContext();

        [ActionName("DeleteComment")]
        public ActionResult DeleteComment()
        {
            int deleteCommentId;
            deleteCommentId = int.Parse(Request.Form["currentCommentID"]);
            int pictureId = int.Parse(Request.Form["CurrentPictureID"]);
            Comment commentToDelete = db.Comments.Find(deleteCommentId);
            NotificationsController.RemoveCommentNotification(commentToDelete, pictureId);
            db.Comments.Remove(commentToDelete);
            db.SaveChanges();
            return RedirectToAction("Details/" + pictureId, "Books");
        }

        [HttpPost, ActionName("AddComment")]
        public ActionResult AddComment()
        {
            string newComment;
            newComment = Request.Form["NewComment"];
            int bookId = int.Parse(Request.Form["currentID"]);
            if (newComment == "")
            {
                return RedirectToAction("Details/" + bookId, "Books");
            }
            int userId = AuthManager.GetAuthenticated().Id;
            Comment comment = new Comment()
            {
                UserId = userId,
                Content = newComment,
                Book = db.Books.Find(bookId)
            };

            db.Comments.Add(comment);
            db.SaveChanges();

            NotificationsController.AddCommentNotification(bookId, userId);
            return RedirectToAction("Details/" + bookId, "Books");
        }
    }
    
}