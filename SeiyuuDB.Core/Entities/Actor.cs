using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Core.Entities {
  /// <summary>
  /// 声優
  /// </summary>
  [Table(Name = "actors")]
  public sealed class Actor : ISeiyuuEntity<Actor> {
    /// <summary>
    /// 声優ID
    /// </summary>
    [Column(Name = "id", CanBeNull = false, DbType = "INT", IsPrimaryKey = true)]
    public int Id { get; set; } = -1;

    /// <summary>
    /// 性
    /// </summary>
    [Column(Name = "last_name", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    public string LastName { get; private set; }

    /// <summary>
    /// 名
    /// </summary>
    [Column(Name = "first_name", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    public string FirstName { get; private set; }

    /// <summary>
    /// 性かな
    /// </summary>
    [Column(Name = "last_name_kana", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    public string LastNameKana { get; private set; }

    /// <summary>
    /// 名かな
    /// </summary>
    [Column(Name = "first_name_kana", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    public string FirstNameKana { get; private set; }

    /// <summary>
    /// 性ローマ字
    /// </summary>
    [Column(Name = "last_name_romaji", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    public string LastNameRomaji { get; private set; }

    /// <summary>
    /// 名ローマ字
    /// </summary>
    [Column(Name = "first_name_romaji", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    public string FirstNameRomaji { get; private set; }

    /// <summary>
    /// 名前
    /// </summary>
    public string Name {
      get {
        if (string.IsNullOrEmpty(FirstName)) {
          return LastName;
        } else {
          return LastName + " " + FirstName;
        }
      }
    }

    /// <summary>
    /// 名前短縮形
    /// </summary>
    public string ShortName => LastName + FirstName;

    /// <summary>
    /// 名前かな
    /// </summary>
    public string NameKana {
      get {
        if (string.IsNullOrEmpty(FirstNameKana) && string.IsNullOrEmpty(LastNameKana)) {
          return null;
        } else if (string.IsNullOrEmpty(FirstNameKana)) {
          return LastNameKana;
        } else if (string.IsNullOrEmpty(LastNameKana)) {
          return FirstNameKana;
        } else {
          return LastNameKana + " " + FirstNameKana;
        }
      }
    }

    /// <summary>
    /// 名前かな短縮形
    /// </summary>
    public string ShortNameKana => LastNameKana + FirstNameKana;

    /// <summary>
    /// 名前ローマ字
    /// </summary>
    public string NameRomaji {
      get {
        if (string.IsNullOrEmpty(FirstNameRomaji) && string.IsNullOrEmpty(LastNameRomaji)) {
          return null;
        } else if (string.IsNullOrEmpty(FirstNameRomaji)) {
          return LastNameRomaji;
        } else if (string.IsNullOrEmpty(LastNameRomaji)) {
          return FirstNameRomaji;
        } else {
          return LastNameRomaji + " " + FirstNameRomaji;
        }
      }
    }

    /// <summary>
    /// 名前ローマ字短縮形
    /// </summary>
    public string ShortNameRomaji => LastNameRomaji + FirstNameRomaji;

    /// <summary>
    /// ニックネーム
    /// </summary>
    [Column(Name = "nickname", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    public string Nickname { get; private set; }

    [Column(Name = "gender_id", CanBeNull = true, DbType = "INT")]
    private int? _genderId;

    /// <summary>
    /// 性別
    /// </summary>
    public Gender? Gender {
      get {
        if (_genderId.HasValue) {
          return (Gender)Enum.ToObject(typeof(Gender), _genderId.Value);
        } else {
          return null;
        }
      }
      private set {
        if (value.HasValue) {
          _genderId = (int)value.Value;
        } else {
          _genderId = null;
        }
      }
    }

    [Column(Name = "birthdate", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    private string _birthdate;

    /// <summary>
    /// 生年月日
    /// </summary>
    public Birthdate Birthdate {
      get {
        return new Birthdate(_birthdate);
      }
      private set {
        _birthdate = value.StorageString;
      }
    }

    [Column(Name = "blood_type_id", CanBeNull = true, DbType = "INT")]
    private int? _bloodTypeId;

    /// <summary>
    /// 血液型
    /// </summary>
    public BloodType? BloodType {
      get {
        if (_bloodTypeId.HasValue) {
          return (BloodType)Enum.ToObject(typeof(BloodType), _bloodTypeId.Value);
        } else {
          return null;
        }
      }
      private set {
        if (value.HasValue) {
          _bloodTypeId = (int)value.Value;
        } else {
          _bloodTypeId = null;
        }
      }
    }

    /// <summary>
    /// 身長
    /// </summary>
    [Column(Name = "height", CanBeNull = true, DbType = "INT")]
    public int? Height { get; private set; }

    /// <summary>
    /// 出身地
    /// </summary>
    [Column(Name = "hometown", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    public string Hometown { get; private set; }

    /// <summary>
    /// デビュー年
    /// </summary>
    [Column(Name = "debut_year", CanBeNull = true, DbType = "INT")]
    public int? DebutYear { get; private set; }

    /// <summary>
    /// 配偶者名
    /// </summary>
    [Column(Name = "spouse_name", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    public string SpouseName { get; private set; }

    /// <summary>
    /// 事務所ID
    /// </summary>
    [Column(Name = "agency_id", CanBeNull = true, DbType = "INT")]
    public int? AgencyId { get; private set; }

    private EntityRef<Company> _agency;

    /// <summary>
    /// 事務所
    /// </summary>
    [Association(Storage = "_agency", ThisKey = "AgencyId", IsForeignKey = true)]
    public Company Agency {
      get { return _agency.Entity; }
      private set { _agency.Entity = value; }
    }

    /// <summary>
    /// 写真URL
    /// </summary>
    [Column(Name = "picture_url", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    public string PictureUrl { get; private set; }

    [Column(Name = "is_favorite", CanBeNull = false, DbType = "INT")]
    private int _isFavorite;

    /// <summary>
    /// お気に入りフラグ
    /// </summary>
    public bool IsFavorite {
      get {
        return _isFavorite == 1;
      }
      set {
        _isFavorite = value ? 1 : 0;
      }
    }

    [Column(Name = "is_completed", CanBeNull = false, DbType = "INT")]
    private int _isCompleted;

    /// <summary>
    /// 編集完了フラグ
    /// </summary>
    public bool IsCompleted {
      get {
        return _isCompleted == 1;
      }
      set {
        _isCompleted = value ? 1 : 0;
      }
    }

    [Column(Name = "created_at", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    private string _createdAt;

    /// <summary>
    /// 作成日時
    /// </summary>
    public DateTime CreatedAt {
      get { return DateTime.Parse(_createdAt); }
      set { _createdAt = value.ToString(); }
    }

    [Column(Name = "updated_at", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    private string _updatedAt;

    /// <summary>
    /// 更新日時
    /// </summary>
    public DateTime UpdatedAt {
      get { return DateTime.Parse(_updatedAt); }
      set { _updatedAt = value.ToString(); }
    }

    /// <summary>
    /// キャラクタ一覧
    /// </summary>
    [Association(OtherKey = "ActorId", Storage = "_characters")]
    public EntitySet<Character> Characters {
      get { return _characters; }
      set { _characters.Assign(value); }
    }
    private EntitySet<Character> _characters;

    /// <summary>
    /// ラジオ声優一覧
    /// </summary>
    [Association(OtherKey = "ActorId", Storage = "_radiosActors")]
    public EntitySet<RadioActor> RadiosActors {
      get { return _radiosActors; }
      set { _radiosActors.Assign(value); }
    }
    private EntitySet<RadioActor> _radiosActors;

    /// <summary>
    /// その他出演一覧
    /// </summary>
    [Association(OtherKey = "ActorId", Storage = "_otherAppearances")]
    public EntitySet<OtherAppearance> OtherAppearances {
      get { return _otherAppearances; }
      set { _otherAppearances.Assign(value); }
    }
    private EntitySet<OtherAppearance> _otherAppearances;

    /// <summary>
    /// 外部リンク一覧
    /// </summary>
    [Association(OtherKey = "ActorId", Storage = "_externalLinks")]
    public EntitySet<ExternalLink> ExternalLinks {
      get { return _externalLinks; }
      set { _externalLinks.Assign(value); }
    }
    private EntitySet<ExternalLink> _externalLinks;

    /// <summary>
    /// ノート一覧
    /// </summary>
    [Association(OtherKey = "ActorId", Storage = "_notes")]
    public EntitySet<Note> Notes {
      get { return _notes; }
      set { _notes.Assign(value); }
    }
    private EntitySet<Note> _notes;

    public Actor() {
      _characters = new EntitySet<Character>();
      _radiosActors = new EntitySet<RadioActor>();
      _otherAppearances = new EntitySet<OtherAppearance>();
      _externalLinks = new EntitySet<ExternalLink>();
      _notes = new EntitySet<Note>();
    }

    public Actor(string lastName, string firstName, string lastNameKana, string firstNameKana,
      string lastNameRomaji, string firstNameRomaji, string nickname, Gender? gender, Birthdate birthdate,
      BloodType? bloodType, int? height, string hometown, int? debutYear, string spouseName,
      Company agency, string pictureUrl, bool isFavorite, bool isCompleted) : this() {
      LastName = lastName;
      FirstName = firstName;
      LastNameKana = lastNameKana;
      FirstNameKana = firstNameKana;
      LastNameRomaji = lastNameRomaji;
      FirstNameRomaji = firstNameRomaji;
      Nickname = nickname;
      Gender = gender;
      Birthdate = birthdate;
      BloodType = bloodType;
      Height = height;
      Hometown = hometown;
      DebutYear = debutYear;
      SpouseName = spouseName;
      Agency = agency;
      AgencyId = agency?.Id;
      PictureUrl = pictureUrl;
      IsFavorite = isFavorite;
      IsCompleted = isCompleted;
    }

    public void Replace(Actor entity) {
      LastName = entity.LastName;
      FirstName = entity.FirstName;
      LastNameKana = entity.LastNameKana;
      FirstNameKana = entity.FirstNameKana;
      LastNameRomaji = entity.LastNameRomaji;
      FirstNameRomaji = entity.FirstNameRomaji;
      Nickname = entity.Nickname;
      Gender = entity.Gender;
      Birthdate = entity.Birthdate;
      BloodType = entity.BloodType;
      Height = entity.Height;
      Hometown = entity.Hometown;
      DebutYear = entity.DebutYear;
      SpouseName = entity.SpouseName;
      Agency = entity.Agency;
      AgencyId = entity.AgencyId;
      PictureUrl = entity.PictureUrl;
      IsFavorite = entity.IsFavorite;
      IsCompleted = entity.IsCompleted;
    }

    public bool IsReadyEntity() {
      return IsReadyEntityWithoutId() && Id != -1;
    }

    public bool IsReadyEntityWithoutId() {
      return Name != null;
    }

    public override bool Equals(object obj) {
      if (obj is Actor item) {
        return item.Name == Name;
      }
      return false;
    }

    public override int GetHashCode() {
      return Name.GetHashCode();
    }

    public static bool operator ==(Actor a, Actor b) {
      return Equals(a, b);
    }

    public static bool operator !=(Actor a, Actor b) {
      return !Equals(a, b);
    }

    public override string ToString() {
      return $"Id: {Id}, Name: {Name}, NameKana: {NameKana ?? "NULL"}, NameRomaji: {NameRomaji ?? "NULL"}, "
        + $"Nickname: {Nickname ?? "NULL"}, Gender: ({Gender?.ToString() ?? "NULL"}), "
        + $"Birthdate: {Birthdate.ToString() ?? "NULL"}, BloodType: ({BloodType?.ToString() ?? "NULL"}), "
        + $"Height: {Height?.ToString() ?? "NULL"}, Hometown: {Hometown ?? "NULL"}, "
        + $"DebutYear: {DebutYear?.ToString() ?? "NULL"}, SpouseName: {SpouseName ?? "NULL"}, "
        + $"Agency: ({Agency?.ToString() ?? "NULL"}), PictureUrl: {PictureUrl ?? "NULL"}, "
        + $"IsFavorite: {IsFavorite}, IsCompleted: {IsCompleted}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
