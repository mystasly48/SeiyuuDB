using System;

namespace SeiyuuDB.Core.Entities {
  /// <summary>
  /// 生年月日
  /// </summary>
  public class Birthdate {
    private static readonly DateTime BASE_DATE = new DateTime(1, 1, 1);

    /// <summary>
    /// 誕生年
    /// </summary>
    public int? Year { get; set; }

    /// <summary>
    /// 誕生月
    /// </summary>
    public int? Month { get; set; }

    /// <summary>
    /// 誕生日
    /// </summary>
    public int? Day { get; set; }

    /// <summary>
    /// DB保存用生年月日文字列
    /// </summary>
    public string StorageString {
      get {
        if (!Year.HasValue && !Month.HasValue && !Day.HasValue) {
          return null;
        } else {
          var year = Year.HasValue ? $"{Year, 4}" : "????";
          var month = Month.HasValue ? $"{Month, 2}" : "??";
          var day = Day.HasValue ? $"{Day, 2}" : "??";
          return $"{year}/{month}/{day}";
        }
      }
    }

    /// <summary>
    /// 年齢
    /// </summary>
    public int? Age {
      get {
        if (Year.HasValue && Month.HasValue && Day.HasValue) {
          var birthdate = new DateTime(Year.Value, Month.Value, Day.Value);
          var span = DateTime.Now - birthdate;
          if (span.Days >= 0) {
            var age = (BASE_DATE + span).Year - 1;
            return age;
          } else {
            var age = (BASE_DATE - span).Year;
            return -age;
          }
        } else {
          return null;
        }
      }
    }

    /// <summary>
    /// 今日が誕生日であるかどうか
    /// </summary>
    public bool IsToday {
      get {
        if (Month.HasValue && Day.HasValue) {
          var today = DateTime.Today;
          return today.Month == Month && today.Day == Day;
        } else {
          return false;
        }
      }
    }

    public Birthdate(int? year, int? month, int? day) {
      Year = year;
      Month = month;
      Day = day;
    }

    public Birthdate(string date) {
      if (!string.IsNullOrEmpty(date)) {
        var values = date.Split('/');
        Year = values[0] == "????" ? null : (int?)int.Parse(values[0]);
        Month = values[1] == "??" ? null : (int?)int.Parse(values[1]);
        Day = values[2] == "??" ? null : (int?)int.Parse(values[2]);
      }
    }

    public override string ToString() {
      if (!Year.HasValue && !Month.HasValue && !Day.HasValue) {
        return null;
      } else {
        var year = Year.HasValue ? $"{Year, 4}年" : "";
        var month = Month.HasValue ? $"{Month, 2}月" : "";
        var day = Day.HasValue ? $"{Day, 2}日" : "";
        return $"{year}{month}{day}";
      }
    }
  }
}
