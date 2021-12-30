using Avalonia.Controls;
using System.IO;
using System.Net;

namespace HtmlBox
{
    public static class Downloader
    {
        public static void LoadImage(ref Image control, string src)
        {
            if (control == null || string.IsNullOrEmpty(src))
                return;

            using (WebClient client = new())
            {
                var bytes = client.DownloadData(src);

                Stream stream = new MemoryStream(bytes);

                var image = new Avalonia.Media.Imaging.Bitmap(stream);

                control.Source = image;
            }
        }
    }
}
