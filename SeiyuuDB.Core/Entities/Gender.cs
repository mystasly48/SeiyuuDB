using System;
using System.Collections.Generic;

namespace SeiyuuDB.Core.Entities {
  /// <summary>
  /// 性別
  /// </summary>
  public class Gender {
    private const int MALE_ID = 1;
    private const string MALE_NAME = "男性";

    private const int FEMALE_ID = 2;
    private const string FEMALE_NAME = "女性";

    /// <summary>
    /// 未選択
    /// </summary>
    public static Gender Empty => new Gender(null);

    /// <summary>
    /// 男性
    /// </summary>
    public static Gender Male => new Gender(MALE_ID);

    /// <summary>
    /// 女性
    /// </summary>
    public static Gender Female => new Gender(FEMALE_ID);

    /// <summary>
    /// 性別一覧
    /// </summary>
    public static List<Gender> Genders => new List<Gender>() { Empty, Male, Female };

    /// <summary>
    /// 性別ID
    /// </summary>
    public int? Id { get; }

    /// <summary>
    /// 表示名
    /// </summary>
    public string Name {
      get {
        switch (this.Id) {
          case MALE_ID:
            return MALE_NAME;
          case FEMALE_ID:
            return FEMALE_NAME;
          default:
            return "";
        }
      }
    }

    public Gender(int? id) {
      if (id == null || (1 <= id && id <= 2)) {
        this.Id = id;
      } else {
        throw new ArgumentOutOfRangeException();
      }
    }

    public override string ToString() {
      return Name == "" ? null : Name;
    }

    public static bool operator ==(Gender a, Gender b) {
      return Equals(a, b);
    }

    public static bool operator !=(Gender a, Gender b) {
      return !Equals(a, b);
    }

    public override bool Equals(object obj) {
      if (obj is Gender other) {
        return this.Id == other.Id;
      }
      return false;
    }

    public override int GetHashCode() {
      return this.Id.GetHashCode();
    }
  }
}
