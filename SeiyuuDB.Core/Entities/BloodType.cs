using System;
using System.Collections.Generic;

namespace SeiyuuDB.Core.Entities {
  /// <summary>
  /// 血液型
  /// </summary>
  public class BloodType {
    private const int A_ID = 1;
    private const string A_NAME = "A型";

    private const int B_ID = 2;
    private const string B_NAME = "B型";

    private const int O_ID = 3;
    private const string O_NAME = "O型";

    private const int AB_ID = 4;
    private const string AB_NAME = "AB型";

    /// <summary>
    /// 未選択
    /// </summary>
    public static BloodType Empty => new BloodType(null);

    /// <summary>
    /// A型
    /// </summary>
    public static BloodType A => new BloodType(A_ID);

    /// <summary>
    /// B型
    /// </summary>
    public static BloodType B => new BloodType(B_ID);

    /// <summary>
    /// O型
    /// </summary>
    public static BloodType O => new BloodType(O_ID);

    /// <summary>
    /// AB型
    /// </summary>
    public static BloodType AB => new BloodType(AB_ID);

    /// <summary>
    /// 血液型一覧
    /// </summary>
    public static List<BloodType> BloodTypes => new List<BloodType>() { Empty, A, B, O, AB };

    /// <summary>
    /// 血液型ID
    /// </summary>
    public int? Id { get; }

    /// <summary>
    /// 表示名
    /// </summary>
    public string Name {
      get {
        switch (this.Id) {
          case A_ID:
            return A_NAME;
          case B_ID:
            return B_NAME;
          case O_ID:
            return O_NAME;
          case AB_ID:
            return AB_NAME;
          default:
            return "";
        }
      }
    }

    public BloodType(int? id) {
      if (id == null || (1 <= id && id <= 4)) {
        this.Id = id;
      } else {
        throw new ArgumentOutOfRangeException();
      }
    }

    public override string ToString() {
      return Name == "" ? null : Name;
    }

    public static bool operator ==(BloodType a, BloodType b) {
      return Equals(a, b);
    }

    public static bool operator !=(BloodType a, BloodType b) {
      return !Equals(a, b);
    }

    public override bool Equals(object obj) {
      if (obj is BloodType other) {
        return this.Id == other.Id;
      }
      return false;
    }

    public override int GetHashCode() {
      return this.Id.GetHashCode();
    }
  }
}
