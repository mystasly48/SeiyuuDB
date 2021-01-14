using System;

namespace SeiyuuDB.Entities {
  public class Birthdate {
    private static readonly DateTime BASE_DATE = new DateTime(1, 1, 1);

    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }

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

    public int? Age {
      get {
        if (Year.HasValue && Month.HasValue && Day.HasValue) {
          var birthdate = new DateTime(Year.Value, Month.Value, Day.Value);
          var span = DateTime.Now - birthdate;
          var age = (BASE_DATE + span).Year - 1;
          return age;
        } else {
          return null;
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
