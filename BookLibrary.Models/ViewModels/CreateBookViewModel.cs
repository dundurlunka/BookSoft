namespace BookLibrary.Models.ViewModels
{
    using System.Web;

    public class CreateBookViewModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string TagsString { get; set; }

        public HttpPostedFileBase CoverPictureFile { get; set; }
    }
}