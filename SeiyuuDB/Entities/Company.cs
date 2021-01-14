using Newtonsoft.Json;
using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Entities {
  [Table(Name = "Company")]
  [JsonObject("Company")]
  public sealed class Company : ISeiyuuEntity<Company> {
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

    [Column(Name = "name", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    [JsonProperty("name")]
    public string Name { get; private set; }

    [Column(Name = "name_kana", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    [JsonProperty("name_kana")]
    public string NameKana { get; private set; }

    [Column(Name = "type_id", CanBeNull = false, DbType = "INT")]
    [JsonProperty("type_id")]
    private int _companyTypeId;

    [JsonIgnore]
    public CompanyType CompanyType {
      get {
        return (CompanyType)Enum.ToObject(typeof(CompanyType), _companyTypeId);
      }
      private set {
        _companyTypeId = (int)value;
      }
    }

    [Column(Name = "url", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    [JsonProperty("url")]
    public string Url { get; private set; }

    [Column(Name = "created_at", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    [JsonProperty("created_at")]
    private string _createdAt;

    [JsonIgnore]
    public DateTime CreatedAt {
      get { return DateTime.Parse(_createdAt); }
      set { _createdAt = value.ToString(); }
    }

    [Column(Name = "updated_at", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    [JsonProperty("updated_at")]
    private string _updatedAt;

    [JsonIgnore]
    public DateTime UpdatedAt {
      get { return DateTime.Parse(_updatedAt); }
      set { _updatedAt = value.ToString(); }
    }

    private EntitySet<Actor> _actors;
    [Association(OtherKey = "AgencyId", Storage = "_actors")]
    public EntitySet<Actor> Actors {
      get { return _actors; }
      set { _actors.Assign(value); }
    }

    private EntitySet<Radio> _radios;
    [Association(OtherKey = "StationId", Storage = "_radios")]
    public EntitySet<Radio> Radios {
      get { return _radios; }
      set { _radios.Assign(value); }
    }

    public Company() {
      _actors = new EntitySet<Actor>();
      _radios = new EntitySet<Radio>();
    }

    public Company(string name, string name_kana, CompanyType company_type, string url) : this() {
      Name = name;
      NameKana = name_kana;
      CompanyType = company_type;
      Url = url;
    }

    public void Replace(Company entity) {
      Name = entity.Name;
      NameKana = entity.NameKana;
      CompanyType = entity.CompanyType;
      Url = entity.Url;
    }

    public bool IsReadyEntity() {
      return IsReadyEntityWithoutId() && Id != -1;
    }

    public bool IsReadyEntityWithoutId() {
      return Name != null;
    }

    public override bool Equals(object obj) {
      if (obj is Company item) {
        return item.Name == Name;
      }
      return false;
    }

    public override int GetHashCode() {
      return Name.GetHashCode();
    }

    public static bool operator ==(Company a, Company b) {
      return Equals(a, b);
    }

    public static bool operator !=(Company a, Company b) {
      return !Equals(a, b);
    }

    public override string ToString() {
      return $"Id: {Id}, Name: {Name}, NameKana: {NameKana}, CompanyType: {CompanyType}, Url: {Url ?? "NULL"}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
