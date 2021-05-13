using SeiyuuDB.Wpf.Properties;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace SeiyuuDB.Wpf.Utils {
  public static class ImageHelper {
    public static BitmapImage NoImage => BitmapToBitmapImage(Resources.NoImage);

    public static BitmapImage UrlToBitmapImage(string url) {
      var bitmapImage = new BitmapImage();
      bitmapImage.BeginInit();
      bitmapImage.UriSource = new Uri(url);
      bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
      bitmapImage.EndInit();
      bitmapImage.Freeze();
      return bitmapImage;
    }

    public static BitmapImage BitmapToBitmapImage(Bitmap bitmap) {
      using (var memory = new MemoryStream()) {
        bitmap.Save(memory, ImageFormat.Jpeg);
        memory.Position = 0;

        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.StreamSource = memory;
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.CreateOptions = BitmapCreateOptions.None;
        bitmapImage.EndInit();
        bitmapImage.Freeze();

        return bitmapImage;
      }
    }
  }
}
