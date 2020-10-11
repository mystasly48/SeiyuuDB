using Newtonsoft.Json;
using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Entities {
  [Table(Name = "Actor")]
  [JsonObject("Actor")]
  public sealed class Actor : ISeiyuuEntity<Actor> {
    [Column(Name = "id", CanBeNull = false, DbType = "INT", IsPrimaryKey = true)]
    [JsonIgnore]
    public int Id { get; set; } = -1;
    
    // For CosmosDB
    //[JsonProperty("id")]
    //private string _idString {
    //  get {
    //    return Id.ToString();
    //  }
    //  set {
    //    Id = int.Parse(value);
    //  }
    //}

    [Column(Name = "last_name", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    [JsonProperty("last_name")]
    public string LastName { get; private set; }

    [Column(Name = "first_name", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    [JsonProperty("first_name")]
    public string FirstName { get; private set; }

    [Column(Name = "last_name_kana", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    [JsonProperty("last_name_kana")]
    public string LastNameKana { get; private set; }

    [Column(Name = "first_name_kana", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    [JsonProperty("first_name_kana")]
    public string FirstNameKana { get; private set; }

    [Column(Name = "last_name_romaji", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    [JsonProperty("last_name_romaji")]
    public string LastNameRomaji { get; private set; }

    [Column(Name = "first_name_romaji", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    [JsonProperty("first_name_romaji")]
    public string FirstNameRomaji { get; private set; }

    [JsonIgnore]
    public string Name => LastName + " " + FirstName;

    [JsonIgnore]
    public string NameKana => LastNameKana + " " + FirstNameKana;

    [JsonIgnore]
    public string NameRomaji => LastNameRomaji + " " + FirstNameRomaji;

    [Column(Name = "nickname", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    [JsonProperty("nickname")]
    public string Nickname { get; private set; }

    [Column(Name = "gender_id", CanBeNull = true, DbType = "INT")]
    [JsonProperty("gender_id")]
    private int? _genderId;

    [JsonIgnore]
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
    [JsonProperty("birthdate")]
    private string _birthdate;

    [JsonIgnore]
    public DateTime? Birthdate {
      get {
        if (_birthdate is null) {
          return null;
        } else {
          if (_birthdate.Length <= 5) {
            return DateTime.Parse("0001/" + _birthdate);
          } else {
            return DateTime.Parse(_birthdate);
          }
        }
      }
      private set {
        if (value.HasValue) {
          if (value.Value.Year == 1) {
            _birthdate = value.Value.ToString("MM/dd");
          } else {
            _birthdate = value.Value.ToString("yyyy/MM/dd");
          }
        } else {
          _birthdate = null;
        }
      }
    }

    [JsonIgnore]
    public string BirthdateString {
      get {
        return Birthdate.HasValue
          ? (Birthdate.Value.Year == 1 ? Birthdate.Value.ToString(" M月 d日") : Birthdate.Value.ToString("yyyy年 M月 d日"))
          : null;
      }
    }

    [Column(Name = "blood_type_id", CanBeNull = true, DbType = "INT")]
    [JsonProperty("blood_type_id")]
    private int? _bloodTypeId;

    [JsonIgnore]
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

    [Column(Name = "height", CanBeNull = true, DbType = "INT")]
    [JsonProperty("height")]
    public int? Height { get; private set; }

    [Column(Name = "hometown", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    [JsonProperty("hometown")]
    public string Hometown { get; private set; }

    [Column(Name = "debut", CanBeNull = true, DbType = "INT")]
    [JsonProperty("debut")]
    public int? Debut { get; private set; }

    [Column(Name = "spouse", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    [JsonProperty("spouse")]
    public string Spouse { get; private set; }

    [Column(Name = "agency_id", CanBeNull = true, DbType = "INT")]
    [JsonProperty("agency_id")]
    public int? AgencyId { get; private set; }

    private EntityRef<Company> _agency;
    [Association(Storage = "_agency", ThisKey = "AgencyId", IsForeignKey = true)]
    [JsonIgnore]
    public Company Agency {
      get { return _agency.Entity; }
      private set { _agency.Entity = value; }
    }

    [Column(Name = "picture_uri", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    [JsonProperty("picture_uri")]
    public string PictureUri { get; private set; }

    [Column(Name = "created_at", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    [JsonProperty("created_at")]
    private string _createdAt;

    [JsonIgnore]
    public DateTime CreatedAt {
      get { return DateTime.Parse(_createdAt); }
      private set { _createdAt = value.ToString(); }
    }

    [Column(Name = "updated_at", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    [JsonProperty("updated_at")]
    private string _updatedAt;

    [JsonIgnore]
    public DateTime UpdatedAt {
      get { return DateTime.Parse(_updatedAt); }
      set { _updatedAt = value.ToString(); }
    }

    public Actor() { }
    public Actor(string last_name, string first_name, string last_name_kana, string first_name_kana, string last_name_romaji, string first_name_romaji,
      string nickname, Gender? gender, DateTime? birthdate, BloodType? blood_type, int? height, string hometown, int? debut, string spouse,
      Company agency, string picture_uri, DateTime created_at, DateTime updated_at) {
      LastName = last_name;
      FirstName = first_name;
      LastNameKana = last_name_kana;
      FirstNameKana = first_name_kana;
      LastNameRomaji = last_name_romaji;
      FirstNameRomaji = first_name_romaji;
      Nickname = nickname;
      Gender = gender;
      Birthdate = birthdate;
      BloodType = blood_type;
      Height = height;
      Hometown = hometown;
      Debut = debut;
      Spouse = spouse;
      Agency = agency;
      AgencyId = agency?.Id;
      PictureUri = picture_uri;
      CreatedAt = created_at;
      UpdatedAt = updated_at;
    }

    /// <summary>
    /// Replace all of the properties to the entity provided excluding Id, CreatedAt, and UpdatedAt.
    /// </summary>
    /// <param name="entity">Entity</param>
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
      Debut = entity.Debut;
      Spouse = entity.Spouse;
      Agency = entity.Agency;
      AgencyId = entity.AgencyId;
      PictureUri = entity.PictureUri;
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
      return $"Id: {Id}, Name: {Name}, NameKana: {NameKana ?? "NULL"}, NameRomaji: {NameRomaji ?? "NULL"}, Nickname: {Nickname ?? "NULL"}, Gender: ({Gender?.ToString() ?? "NULL"}), Birthdate: {Birthdate?.ToString() ?? "NULL"}, BloodType: ({BloodType?.ToString() ?? "NULL"}), Height: {Height?.ToString() ?? "NULL"}, Hometown: {Hometown ?? "NULL"}, Debut: {Debut?.ToString() ?? "NULL"}, Spouse: {Spouse ?? "NULL"}, Agency: ({Agency?.ToString() ?? "NULL"}), PictureUri: {PictureUri ?? "NULL"}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
