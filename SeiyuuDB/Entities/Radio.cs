using Newtonsoft.Json;
using System;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Entities {
  [Table(Name = "Radio")]
  [JsonObject("Radio")]
  public sealed class Radio : ISeiyuuEntity<Radio> {
    [Column(Name = "id", CanBeNull = false, DbType = "INT", IsPrimaryKey = true)]
    [JsonIgnore]
    public int Id { get; set; } = -1;

    //[JsonProperty("id")]
    //private string _idString {
    //  get {
    //    return Id.ToString();
    //  }
    //  set {
    //    Id = int.Parse(value);
    //  }
    //}

    [Column(Name = "title", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    [JsonProperty("title")]
    public string Title { get; private set; }

    [Column(Name = "station_id", CanBeNull = true, DbType = "INT")]
    [JsonProperty("station_id")]
    public int? StationId { get; private set; }

    [JsonIgnore]
    public Company Station { get; private set; }

    [Column(Name = "since", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    [JsonProperty("since")]
    private string _since;

    [JsonIgnore]
    public DateTime? Since {
      get {
        if (_since is null) {
          return null;
        } else {
          return (DateTime?)DateTime.Parse(_since);
        }
      }
      private set {
        if (value.HasValue) {
          _since = value.Value.ToString();
        } else {
          _since = null;
        }
      }
    }

    [Column(Name = "until", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    [JsonProperty("until")]
    private string _until;

    [JsonIgnore]
    public DateTime? Until {
      get {
        if (_until is null) {
          return null;
        } else {
          return (DateTime?)DateTime.Parse(_until);
        }
      }
      private set {
        if (value.HasValue) {
          _until = value.Value.ToString();
        } else {
          _until = null;
        }
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
      get {
        return DateTime.Parse(_createdAt);
      }
      private set {
        _createdAt = value.ToString();
      }
    }

    [Column(Name = "updated_at", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    [JsonProperty("updated_at")]
    private string _updatedAt;

    [JsonIgnore]
    public DateTime UpdatedAt {
      get {
        return DateTime.Parse(_updatedAt);
      }
      set {
        _updatedAt = value.ToString();
      }
    }

    public Radio() { }

    public Radio(string title, Company station, DateTime? since, DateTime? until, string url, DateTime created_at, DateTime updated_at) {
      Title = title;
      StationId= station.Id;
      Station = station;
      Since = since;
      Until = until;
      Url = url;
      CreatedAt = created_at;
      UpdatedAt = updated_at;
    }

    public Radio(Radio radio, Company station) : this(radio.Title, station, radio.Since, radio.Until, radio.Url, radio.CreatedAt, radio.UpdatedAt) {
      Id = radio.Id;
    }

    public void Replace(Radio entity) {
      Title = entity.Title;
      StationId = entity.StationId;
      Station = entity.Station;
      Since = entity.Since;
      Until = entity.Until;
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
      return $"Id: {Id}, Title: {Title}, Station: ({Station?.ToString() ?? "NULL"}), Since: {Since?.ToString() ?? "NULL"}, Until: {Until?.ToString() ?? "NULL"}, Url: {Url ?? "NULL"}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
