using Newtonsoft.Json;
using System;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Entities {
  [Table(Name = "Company")]
  [JsonObject("Company")]
  public sealed class Company : SeiyuuBaseEntity<Company> {
    [Column(Name = "name", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    [JsonProperty("name")]
    public string Name { get; private set; }

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

    public Company() { }

    public Company(string name, CompanyType company_type, string url, DateTime created_at, DateTime updated_at) {
      Name = name;
      CompanyType = company_type;
      Url = url;
      CreatedAt = created_at;
      UpdatedAt = updated_at;
    }

    public override void Replace(Company entity) {
      Name = entity.Name;
      CompanyType = entity.CompanyType;
      Url = entity.Url;
    }

    public override bool IsReadyEntity() {
      return IsReadyEntityWithoutId() && Id != -1;
    }

    public override bool IsReadyEntityWithoutId() {
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
      return $"Id: {Id}, Name: {Name}, CompanyType: {CompanyType}, Url: {Url ?? "NULL"}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
