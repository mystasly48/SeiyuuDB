using SeiyuuDB.Entities;
using System;

namespace SeiyuuDB.Helpers {
  public static class DatabaseHelper {
    public static void OutputTable<T>(T[] table) where T : class, ISeiyuuEntity<T>, new() {
      Console.WriteLine($"{typeof(T).Name}");
      if (table != null) {
        foreach (var entity in table) {
          Console.WriteLine(entity.ToString());
        }
      }
      Console.WriteLine();
    }
  }
}
