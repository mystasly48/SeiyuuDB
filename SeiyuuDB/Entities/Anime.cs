using Newtonsoft.Json;
using System;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Entities {
  [Table(Name = "Anime")]
  [JsonObject("Anime")]
  public sealed class Anime : SeiyuuBaseEntity<Anime> {
    [Column(Name = "title", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    [JsonProperty("title")]
    public string Title { get; private set; }

    [Column(Name = "year", CanBeNull = false, DbType = "INT")]
    [JsonProperty("year")]
    public int Year { get; private set; }

    [Column(Name = "url", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    [JsonProperty("url")]
    public string Url { get; private set; }

    public Anime() { }
    public Anime(string title, int year, string url, DateTime created_at, DateTime updated_at) {
      Title = title;
      Year = year;
      Url = url;
      CreatedAt = created_at;
      UpdatedAt = updated_at;
    }

    public override void Replace(Anime entity) {
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
      if (obj is Anime item) {
        return item.Title == Title;
      }
      return false;
    }

    public override int GetHashCode() {
      return Title.GetHashCode();
    }

    public static bool operator ==(Anime a, Anime b) {
      return Equals(a, b);
    }

    public static bool operator !=(Anime a, Anime b) {
      return !Equals(a, b);
    }

    public override string ToString() {
      return $"Id: {Id}, Title: {Title}, Year: {Year}, Url: {Url ?? "NULL"}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
