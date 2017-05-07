namespace BookLibrary.Models
{
    using System;

    public enum NotificationType
    {
        Follow,
        BookUpload,
        Comment
    }

    public class Notification
    {
        public Notification()
        {
            this.Date = DateTime.Now;
        }

        public int Id { get; set; }

        public int SenderId { get; set; }

        public virtual User Sender { get; set; }

        public int ReceiverId { get; set; }

        public virtual User Receiver { get; set; }

        public int? BookId { get; set; }

        public virtual Book Book { get; set; }

        public NotificationType Type { get; set; }

        public DateTime Date { get; set; }
    }
}