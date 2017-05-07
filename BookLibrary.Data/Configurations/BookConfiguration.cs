namespace BookLibrary.Web.Configurations
{
    using BookLibrary.Models;
    using System.Data.Entity.ModelConfiguration;

    public class BookConfiguration : EntityTypeConfiguration<Book>
    {
        public BookConfiguration()
        {
            this.Ignore(b => b.CoverFile);

            this.HasMany(p => p.Comments).WithRequired(c => c.Book).WillCascadeOnDelete(false);

            this.HasMany(b => b.Notifications).WithOptional(n => n.Book).WillCascadeOnDelete(false);

            this.HasMany(b => b.Tags).WithMany(t => t.Books).Map(m =>
            {
                m.MapLeftKey("BookId");
                m.MapRightKey("TagId");
                m.ToTable("TagsBooks");
            });
        }
    }
}