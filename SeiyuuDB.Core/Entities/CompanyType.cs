using System;
using System.Collections.Generic;

namespace SeiyuuDB.Core.Entities {
  /// <summary>
  /// 会社種別
  /// </summary>
  public class CompanyType {
    private const int STATION_ID = 1;
    private const string STATION_NAME = "放送局";

    private const int AGENCY_ID = 2;
    private const string AGENCY_NAME = "事務所";

    /// <summary>
    /// 未選択
    /// </summary>
    public static CompanyType Empty => new CompanyType(null);

    /// <summary>
    /// 放送局
    /// </summary>
    public static CompanyType Station => new CompanyType(STATION_ID);

    /// <summary>
    /// 事務所
    /// </summary>
    public static CompanyType Agency => new CompanyType(AGENCY_ID);

    /// <summary>
    /// 会社種別一覧
    /// </summary>
    public static List<CompanyType> CompanyTypes => new List<CompanyType>() { Empty, Station, Agency };

    /// <summary>
    /// 会社種別ID
    /// </summary>
    public int? Id { get; }

    /// <summary>
    /// 表示名
    /// </summary>
    public string Name {
      get {
        switch (this.Id) {
          case STATION_ID:
            return STATION_NAME;
          case AGENCY_ID:
            return AGENCY_NAME;
          default:
            return "";
        }
      }
    }

    public CompanyType(int? id) {
      if (id == null || (1 <= id && id <= 2)) {
        this.Id = id;
      } else {
        throw new ArgumentOutOfRangeException();
      }
    }

    public override string ToString() {
      return Name == "" ? null : Name;
    }

    public static bool operator ==(CompanyType a, CompanyType b) {
      return Equals(a, b);
    }

    public static bool operator !=(CompanyType a, CompanyType b) {
      return !Equals(a, b);
    }

    public override bool Equals(object obj) {
      if (obj is CompanyType other) {
        return this.Id == other.Id;
      }
      return false;
    }

    public override int GetHashCode() {
      return this.Id.GetHashCode();
    }
  }
}
