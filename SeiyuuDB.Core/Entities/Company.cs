using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Core.Entities {
  /// <summary>
  /// 会社
  /// </summary>
  [Table(Name = "companies")]
  public sealed class Company : ISeiyuuEntity<Company> {
    /// <summary>
    /// 会社ID
    /// </summary>
    [Column(Name = "id", CanBeNull = false, DbType = "INT", IsPrimaryKey = true)]
    public int Id { get; set; } = -1;

    /// <summary>
    /// 名前
    /// </summary>
    [Column(Name = "name", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    public string Name { get; private set; }

    /// <summary>
    /// 名前かな
    /// </summary>
    [Column(Name = "name_kana", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    public string NameKana { get; private set; }

    /// <summary>
    /// 別称
    /// </summary>
    [Column(Name = "alias", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    public string Alias { get; private set; }

    /// <summary>
    /// 会社種別ID
    /// </summary>
    [Column(Name = "company_type_id", CanBeNull = false, DbType = "INT")]
    public int CompanyTypeId { get; private set; }

    /// <summary>
    /// 会社種別
    /// </summary>
    public CompanyType CompanyType {
      get {
        return new CompanyType(CompanyTypeId);
      }
      private set {
        CompanyTypeId = value.Id.Value;
      }
    }

    /// <summary>
    /// URL
    /// </summary>
    [Column(Name = "url", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    public string Url { get; private set; }

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
    /// 声優一覧
    /// </summary>
    [Association(OtherKey = "AgencyId", Storage = "_actors")]
    public EntitySet<Actor> Actors {
      get { return _actors; }
      set { _actors.Assign(value); }
    }
    private EntitySet<Actor> _actors;

    /// <summary>
    /// ラジオ一覧
    /// </summary>
    [Association(OtherKey = "StationId", Storage = "_radios")]
    public EntitySet<Radio> Radios {
      get { return _radios; }
      set { _radios.Assign(value); }
    }
    private EntitySet<Radio> _radios;

    public Company() {
      _actors = new EntitySet<Actor>();
      _radios = new EntitySet<Radio>();
    }

    public Company(string name, string nameKana, string alias, CompanyType companyType, string url) : this() {
      Name = name;
      NameKana = nameKana;
      Alias = alias;
      CompanyType = companyType;
      Url = url;
    }

    public void Replace(Company entity) {
      Name = entity.Name;
      NameKana = entity.NameKana;
      Alias = entity.Alias;
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
      return $"Id: {Id}, Name: {Name}, NameKana: {NameKana}, Alias: {Alias}, CompanyType: {CompanyType}, "
        + $"Url: {Url ?? "NULL"}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
