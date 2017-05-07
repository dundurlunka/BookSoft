namespace BookLibrary.Models.ViewModels
{
    using System.Collections.Generic;

    public class SearchViewModel
    {
        public SearchViewModel()
        {
            this.Tags = new HashSet<Tag>();
            this.Users = new HashSet<User>();
        }

        public ICollection<Tag> Tags { get; set; }

        public ICollection<User> Users { get; set; }

        public ICollection<Book> Books { get; set; }

        public bool IsAnythingFound { get; set; }
    }
}