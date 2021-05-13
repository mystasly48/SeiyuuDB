using System.IO;
using System.Reflection;

namespace SeiyuuDB.Wpf.Utils {
  public static class Information {
    public static string Title { get => "SeiyuuDB"; }

    public static string SqliteFilePath => @"D:\Drive\Programming\Csharp\WPF\SeiyuuDB\Databases\SeiyuuDB.db";
    public static string BlobFolderPath => @"D:\Drive\Programming\Csharp\WPF\SeiyuuDB\Databases\Images\";

    public static string SettingsFolder { get => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
    public static string SettingsFile { get => Path.Combine(SettingsFolder, Title + ".xml"); }
  }
}
