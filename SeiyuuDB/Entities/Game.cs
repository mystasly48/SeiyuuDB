using Newtonsoft.Json;
using System;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Entities {
  [Table(Name = "Game")]
  [JsonObject("Game")]
  public sealed class Game : SeiyuuBaseEntity<Game> {
    [Column(Name = "title", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    [JsonProperty("title")]
    public string Title { get; private set; }

    [Column(Name = "year", CanBeNull = false, DbType = "INT")]
    [JsonProperty("year")]
    public int Year { get; private set; }

    [Column(Name = "url", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    [JsonProperty("url")]
    public string Url { get; private set; }

    public Game() { }
    public Game(string title, int year, string url, DateTime created_at, DateTime updated_at) {
      Title = title;
      Year = year;
      Url = url;
      CreatedAt = created_at;
      UpdatedAt = updated_at;
    }

    public override void Replace(Game entity) {
      Title = entity.Title;
      Year = entity.Year;
      Url = entity.Url;
    }

    public override bool IsReadyEntity() {
      return IsReadyEntityWithoutId() && Id != -1;
    }

    public override bool IsReadyEntityWithoutId() {
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
