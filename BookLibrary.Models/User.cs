namespace BookLibrary.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;

    public enum Role
    {
        Reader, Writer, Admin
    }

    public class User
    {
        public User()
        {
            this.Followers = new HashSet<User>();
            this.Following = new HashSet<User>();
            this.Books = new HashSet<Book>();
            this.Comments = new HashSet<Comment>();
            this.Notifications = new HashSet<Notification>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        [Index("IX_User_Username", IsUnique = true)]
        public string Username { get; set; }

        public string Password { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        [Index("IX_User_Email", IsUnique = true)]
        public string Email { get; set; }

        public string RepeatPassword { get; set; }

        public Role Role { get; set; }

        public virtual ICollection<User> Followers { get; set; }

        public virtual ICollection<User> Following { get; set; }

        public byte[] RegisterProfilePicture { get; set; }

        public HttpPostedFileBase ProfilePictureFile { get; set; }

        public virtual ICollection<Book> Books { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
    }
}