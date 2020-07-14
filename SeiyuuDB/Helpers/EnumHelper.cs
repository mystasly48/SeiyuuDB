using SeiyuuDB.Entities;
using System;
using System.Collections.Generic;

namespace SeiyuuDB.Helpers {
  public static class EnumHelper {
    public static readonly Dictionary<CompanyType, string> CompanyTypeDisplayNames = new Dictionary<CompanyType, string>() {
      { CompanyType.Agency, "事務所" },
      { CompanyType.Station, "放送局" }
    };

    public static readonly Dictionary<Gender, string> GenderDisplayNames = new Dictionary<Gender, string>() {
      { Gender.Male, "男性" },
      { Gender.Female, "女性" }
    };

    public static bool IsDefined<T>(int n) {
      return Enum.IsDefined(typeof(T), n);
    }

    public static string DisplayName(CompanyType? e) {
      if (e.HasValue && IsDefined<CompanyType>((int)e)) {
        return CompanyTypeDisplayNames[e.Value];
      } else {
        return null;
      }
    }

    public static string DisplayName(Gender? e) {
      if (e.HasValue && IsDefined<Gender>((int)e)) {
        return GenderDisplayNames[e.Value];
      } else {
        return null;
      }
    }
  }
}
