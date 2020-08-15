using Newtonsoft.Json;
using System;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Entities {
  [Table(Name = "Actor")]
  [JsonObject("Actor")]
  public sealed class Actor : SeiyuuBaseEntity<Actor> {
    [Column(Name = "name", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    [JsonProperty("name")]
    public string Name { get; private set; }

    [Column(Name = "hepburn", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    [JsonProperty("hepburn")]
    public string Hepburn { get; private set; }

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

    [JsonIgnore]
    public Company Agency { get; private set; }

    [Column(Name = "picture_uri", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    [JsonProperty("picture_uri")]
    public string PictureUri { get; private set; }

    public Actor() { }

    public Actor(string name, string hepburn, string nickname, Gender? gender, DateTime? birthdate, BloodType? blood_type, int? height, string hometown, int? debut, string spouse, Company agency, string picture_uri, DateTime created_at, DateTime updated_at) {
      Name = name;
      Hepburn = hepburn;
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

    public Actor(Actor actor, Company agency) : this(actor.Name, actor.Hepburn, actor.Nickname, actor.Gender, actor.Birthdate, actor.BloodType, actor.Height, actor.Hometown, actor.Debut, actor.Spouse, agency, actor.PictureUri, actor.CreatedAt, actor.UpdatedAt) {
      Id = actor.Id;
    }

    /// <summary>
    /// Replace all of the properties to the entity provided excluding Id, CreatedAt, and UpdatedAt.
    /// </summary>
    /// <param name="entity">Entity</param>
    public override void Replace(Actor entity) {
      Name = entity.Name;
      Hepburn = entity.Hepburn;
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

    public override bool IsReadyEntity() {
      return IsReadyEntityWithoutId() && Id != -1;
    }

    public override bool IsReadyEntityWithoutId() {
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
      return $"Id: {Id}, Name: {Name}, Hepburn: {Hepburn ?? "NULL"}, Nickname: {Nickname ?? "NULL"}, Gender: ({Gender?.ToString() ?? "NULL"}), Birthdate: {Birthdate?.ToString() ?? "NULL"}, BloodType: ({BloodType?.ToString() ?? "NULL"}), Height: {Height?.ToString() ?? "NULL"}, Hometown: {Hometown ?? "NULL"}, Debut: {Debut?.ToString() ?? "NULL"}, Spouse: {Spouse ?? "NULL"}, Agency: ({Agency?.ToString() ?? "NULL"}), PictureUri: {PictureUri ?? "NULL"}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
