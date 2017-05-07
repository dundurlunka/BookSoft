namespace BookLibrary.Web.Configurations
{
    using BookLibrary.Models;
    using System.Data.Entity.ModelConfiguration;

    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            this.HasMany(u => u.Followers).WithMany(f => f.Following).Map(uf =>
            {
                uf.MapLeftKey("UserId");
                uf.MapRightKey("FollowerId");
                uf.ToTable("UsersFollowers");
            });

            this.HasMany(u => u.Books).WithRequired(b => b.User);

            this.Ignore(u => u.RepeatPassword);
            this.Ignore(u => u.ProfilePictureFile);

            this.HasMany(u => u.Comments)
            .WithRequired(c => c.User);

            this.HasMany(u => u.Notifications).WithRequired(n => n.Receiver).WillCascadeOnDelete(false);
            this.HasMany(u => u.Notifications).WithRequired(n => n.Sender).WillCascadeOnDelete(false);
        }
    }
}