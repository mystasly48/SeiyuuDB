using System;
using System.Linq;

namespace SeiyuuDB.Core.Helpers {
  public class StringHelper {
    /// <summary>
    /// 文字列配列の空でない文字列を、区切り文字を挿入して連結します。
    /// </summary>
    /// <param name="separator">区切り文字</param>
    /// <param name="values">文字列配列</param>
    /// <returns>連結した文字列</returns>
    public static string Join(string separator, params string[] values) {
      if (separator == null) {
        throw new ArgumentNullException("区切り文字はnullにできません");
      }
      if (values == null || values.Length == 0) {
        return null;
      }

      string[] nonEmptyValues = values.Where(str => !string.IsNullOrEmpty(str)).ToArray();
      if (nonEmptyValues.Length == 0) {
        return null;
      } else {
        return string.Join(separator, nonEmptyValues);
      }
    }
  }
}
