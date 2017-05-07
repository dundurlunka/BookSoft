
namespace BookLibrary.Web.Utilities
{
    using System.IO;
    using System.Web;

    public static class PictureUtilities
    {
        public static byte[] PictureToByteArray(HttpPostedFileBase contentFile)
        {
            MemoryStream stream = new MemoryStream();
            contentFile.InputStream.CopyTo(stream);
            byte[] data = stream.ToArray();
            return data;
        }
    }
}