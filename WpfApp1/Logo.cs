using System;
using System.Windows.Media.Imaging;

namespace TVProgram
{
    public class Logo
    {
        public static BitmapImage LoadLogo(string URL)
        {
            var fullFilePath = URL;
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
            bitmap.EndInit();
            return bitmap;
        }
    }
}
