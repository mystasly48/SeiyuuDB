using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Core.Entities {
  /// <summary>
  /// ラジオ
  /// </summary>
  [Table(Name = "radios")]
  public sealed class Radio : ISeiyuuEntity<Radio> {
    /// <summary>
    /// ラジオID
    /// </summary>
    [Column(Name = "id", CanBeNull = false, DbType = "INT", IsPrimaryKey = true)]
    public int Id { get; set; } = -1;

    /// <summary>
    /// タイトル
    /// </summary>
    [Column(Name = "title", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    public string Title { get; private set; }

    /// <summary>
    /// タイトルかな
    /// </summary>
    [Column(Name = "title_kana", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    public string TitleKana { get; private set; }

    /// <summary>
    /// 別称
    /// </summary>
    [Column(Name = "alias", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    public string Alias { get; private set; }

    /// <summary>
    /// 放送局ID
    /// </summary>
    [Column(Name = "station_id", CanBeNull = true, DbType = "INT")]
    public int? StationId { get; private set; }

    /// <summary>
    /// 放送局
    /// </summary>
    [Association(Storage = "_station", ThisKey = "StationId", IsForeignKey = true)]
    public Company Station {
      get { return _station.Entity; }
      private set { _station.Entity = value; }
    }
    private EntityRef<Company> _station;

    [Column(Name = "started_on", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    private string _startedOn;

    /// <summary>
    /// 配信開始日
    /// </summary>
    public DateTime? StartedOn {
      get {
        if (_startedOn is null) {
          return null;
        } else {
          return (DateTime?)DateTime.Parse(_startedOn);
        }
      }
      private set {
        if (value.HasValue) {
          _startedOn = value.Value.ToString();
        } else {
          _startedOn = null;
        }
      }
    }

    [Column(Name = "ended_on", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    private string _endedOn;

    /// <summary>
    /// 配信終了日
    /// </summary>
    public DateTime? EndedOn {
      get {
        if (_endedOn is null) {
          return null;
        } else {
          return (DateTime?)DateTime.Parse(_endedOn);
        }
      }
      private set {
        if (value.HasValue) {
          _endedOn = value.Value.ToString();
        } else {
          _endedOn = null;
        }
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
    /// ラジオ声優一覧
    /// </summary>
    [Association(OtherKey = "RadioId", Storage = "_radiosActors")]
    public EntitySet<RadioActor> RadiosActors {
      get { return _radiosActors; }
      set { _radiosActors.Assign(value); }
    }
    private EntitySet<RadioActor> _radiosActors;

    public Radio() {
      _radiosActors = new EntitySet<RadioActor>();
    }

    public Radio(string title, string titleKana, string alias, Company station, DateTime? startedOn, DateTime? endedOn, string url) : this() {
      Title = title;
      TitleKana = titleKana;
      Alias = alias;
      StationId= station.Id;
      Station = station;
      StartedOn = startedOn;
      EndedOn = endedOn;
      Url = url;
    }

    public void Replace(Radio entity) {
      Title = entity.Title;
      TitleKana = entity.TitleKana;
      Alias = entity.Alias; 
      StationId = entity.StationId;
      Station = entity.Station;
      StartedOn = entity.StartedOn;
      EndedOn = entity.EndedOn;
      Url = entity.Url;
    }

    public bool IsReadyEntity() {
      return IsReadyEntityWithoutId() && Id != -1;
    }

    public bool IsReadyEntityWithoutId() {
      return Title != null;
    }

    public override bool Equals(object obj) {
      if (obj is Radio item) {
        return item.Title == Title;
      }
      return false;
    }

    public override int GetHashCode() {
      return Title.GetHashCode();
    }

    public static bool operator ==(Radio a, Radio b) {
      return Equals(a, b);
    }

    public static bool operator !=(Radio a, Radio b) {
      return !Equals(a, b);
    }

    public override string ToString() {
      return $"Id: {Id}, Title: {Title}, TitleKana: {TitleKana}, Alias: {Alias}, Station: ({Station?.ToString() ?? "NULL"}), StartedOn: {StartedOn?.ToString() ?? "NULL"}, EndedOn: {EndedOn?.ToString() ?? "NULL"}, Url: {Url ?? "NULL"}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
