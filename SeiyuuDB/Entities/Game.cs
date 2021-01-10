using Newtonsoft.Json;
using SeiyuuDB.Helpers;
using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Entities {
  [Table(Name = "Game")]
  [JsonObject("Game")]
  public sealed class Game : ISeiyuuEntity<Game> {
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

    [Column(Name = "title", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    [JsonProperty("title")]
    public string Title { get; private set; }

    [Column(Name = "year", CanBeNull = false, DbType = "INT")]
    [JsonProperty("year")]
    public int Year { get; private set; }

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

    private EntitySet<GameFilmography> _gameFilmographies;
    [Association(OtherKey = "GameId", Storage = "_gameFilmographies")]
    public EntitySet<GameFilmography> GameFilmographies {
      get { return _gameFilmographies; }
      set { _gameFilmographies.Assign(value); }
    }

    public Game() {
      _gameFilmographies = new EntitySet<GameFilmography>();
    }

    public Game(string title, int year, string url) : this() {
      Title = title;
      Year = year;
      Url = url;
    }

    public void Replace(Game entity) {
      Title = entity.Title;
      Year = entity.Year;
      Url = entity.Url;
    }

    public bool IsReadyEntity() {
      return IsReadyEntityWithoutId() && Id != -1;
    }

    public bool IsReadyEntityWithoutId() {
      return Title != null;
    }

    public override bool Equals(object obj) {
      if (obj is Game item) {
        return item.Title == Title;
      }
      return false;
    }

    public override int GetHashCode() {
      return Title.GetHashCode();
    }

    public static bool operator ==(Game a, Game b) {
      return Equals(a, b);
    }

    public static bool operator !=(Game a, Game b) {
      return !Equals(a, b);
    }

    public override string ToString() {
      return $"Id: {Id}, Title: {Title}, Year: {Year}, Url: {Url ?? "NULL"}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
