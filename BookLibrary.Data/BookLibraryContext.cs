namespace BookLibrary.Data
{
    using BookLibrary.Web.Configurations;
    using BookLibrary.Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class BookLibraryContext : DbContext
    {
        public BookLibraryContext()
            : base("name=BookLibraryContext")
        {
            Database.SetInitializer(new MyInitiliazer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new BookConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
    }

}