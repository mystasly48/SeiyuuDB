using System;

namespace SeiyuuDB.Core.Helpers {
  public static class HelperStaticExtentions {
    public static bool Contains(this string str, string substring, StringComparison comp) {
      if (substring == null) {
        throw new ArgumentNullException("substring cannot be null.", "substring");
      } else if (!Enum.IsDefined(typeof(StringComparison), comp)) {
        throw new ArgumentException("comp is not a member of StringComparison", "comp");
      }

      return str.IndexOf(substring, comp) >= 0;
    }

    public static bool ContainsOriginally(this string value, string query) {
      if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(query)) {
        return false;
      } else {
        value = value.ToComparable();
        query = query.ToComparable();
        return value.Contains(query);
      }
    }

    public static bool ContainsOriginally(this int value, string query) {
      return value.ToString().ContainsOriginally(query);
    }

    public static bool ContainsOriginally(this int? value, string query) {
      return value?.ToString().ContainsOriginally(query) ?? false;
    }

    /// <summary>
    /// 全角を半角、カタカナをひらがな、大文字を小文字、に変換した文字列を返却します
    /// </summary>
    /// <param name="str">対象の文字列</param>
    /// <returns>変換後の文字列</returns>
    private static string ToComparable(this string value) {
      return Kana.ToHiragana(Kana.ToHankaku(value.ToLower()));
    }
  }
}
