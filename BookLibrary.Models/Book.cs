namespace BookLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    public class Book
    {
        public Book()
        {
            this.UploadDate = DateTime.Now;
            this.Comments = new HashSet<Comment>();
            this.Tags = new HashSet<Tag>();
            this.Notifications = new HashSet<Notification>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public virtual User User { get; set; }

        public int UserId { get; set; }

        public int Reads { get; set; }

        public DateTime UploadDate { get; set; }

        public byte[] Cover { get; set; }

        public HttpPostedFileBase CoverFile { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
    }
}