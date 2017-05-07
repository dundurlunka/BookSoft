namespace BookLibrary.Models
{
    using System.Collections.Generic;

    public class Tag
    {
        public Tag()
        {
            this.Books = new HashSet<Book>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}