
namespace BookLibrary.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    public class BookDetailsViewModel
    {
        public BookDetailsViewModel()
        {
            this.Tags = new HashSet<Tag>();
            this.Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public string NewComment { get; set; }

        public DateTime UploadDate { get; set; }

        public int Reads { get; set; }
    }
}
