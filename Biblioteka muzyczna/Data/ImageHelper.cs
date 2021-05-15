using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Biblioteka_muzyczna.Data
{
    static class ImageHelper
    {
        public static byte[] ConvertImageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream memStr = new MemoryStream();
            imageIn.Save(memStr, System.Drawing.Imaging.ImageFormat.Png);
            return memStr.ToArray();
        }

        public static BitmapImage ConvertByteArrayToImage(byte[] byteIn)
        {
            MemoryStream memStr = new MemoryStream(byteIn, 0, byteIn.Length);
            memStr.Write(byteIn, 0, byteIn.Length);

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = memStr;
            image.EndInit();

            return image;
        }
    }
}
