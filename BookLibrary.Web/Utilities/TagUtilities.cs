namespace BookLibrary.Web.Utilities
{
    using System.Linq;
    using BookLibrary.Data;

    public static class TagUtilities
    {
        public static bool IsTagExisting(string tag, BookLibraryContext context)
        {
            return context.Tags.Any(t => t.Name == tag);
        }
    }
}