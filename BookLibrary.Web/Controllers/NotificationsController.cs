namespace BookLibrary.Web.Controllers
{
    using BookLibrary.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using BookLibrary.Data;

    public class NotificationsController : Controller
    {
        private static BookLibraryContext db = new BookLibraryContext();

        

        public static void AddFollowNotification(int senderId, int receiverId)
        {
            var notification = new Notification()
            {
                ReceiverId = receiverId,
                SenderId = senderId,
                Type = (NotificationType)int.Parse("0")
            };

            db.Notifications.Add(notification);
            db.SaveChanges();
        }

        public static void RemoveFollowNotification(int senderId, int receiverId)
        {
           db.Notifications.Remove(db.Notifications.Where(n => n.Type == 0 && n.ReceiverId == receiverId && n.SenderId == senderId).First());
            db.SaveChanges();
        }

        public static void AddCommentNotification(int bookId, int senderId)
        {
            int receiverId = db.Books.Find(bookId).UserId;
            if (receiverId != senderId)
            {
                var notification = new Notification()
                {
                    ReceiverId = receiverId,
                    SenderId = senderId,
                    BookId = bookId,
                    Type = (NotificationType)int.Parse("2")
                };

                db.Notifications.Add(notification);
                db.SaveChanges();
            }
        }

        public static void RemoveCommentNotification(Comment comment, int bookId)
        {
            var receiverId = db.Books.Find(bookId).UserId;
            var senderId = comment.UserId;
            var notification = db.Notifications.Where(n => n.Type.ToString() == "Comment" && n.SenderId == senderId && n.ReceiverId == receiverId).FirstOrDefault();
            if (notification != null)
                db.Notifications.Remove(notification);
            db.SaveChanges();
        }

        public static void AddBookNotification(int bookId, int senderId, List<User> receivers)
        {
            foreach (var user in receivers)
            {
                var notification = new Notification()
                {
                    BookId = bookId,
                    ReceiverId = user.Id,
                    SenderId = senderId,
                    Type = (NotificationType)int.Parse("1")
                };

                db.Notifications.Add(notification);
                db.SaveChanges();
            }
        }

        public static void RemoveBookNotification(int bookId)
        {
            var notifications = db.Notifications.Where(n => n.BookId == bookId).ToList();
            foreach (var notification in notifications)
            {
                db.Notifications.Remove(notification);
            }
            db.SaveChanges();
        }

        public static void RemoveUserNotifications(int userId)
        {
            var notifications = db.Notifications.Where(n => n.ReceiverId == userId || n.SenderId == userId).ToList();

            foreach (var notification in notifications)
            {
                db.Notifications.Remove(notification);
            }
            db.SaveChanges();
        }
    }
}