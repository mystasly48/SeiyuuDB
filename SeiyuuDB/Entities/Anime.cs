﻿using Newtonsoft.Json;
using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Entities {
  [Table(Name = "Anime")]
  [JsonObject("Anime")]
  public sealed class Anime : ISeiyuuEntity<Anime> {
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

    [Column(Name = "title_kana", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    [JsonProperty("title_kana")]
    public string TitleKana { get; private set; }

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

    private EntitySet<AnimeFilmography> _animeFilmographies;
    [Association(OtherKey = "AnimeId", Storage = "_animeFilmographies")]
    public EntitySet<AnimeFilmography> AnimeFilmographies {
      get { return _animeFilmographies; }
      set { _animeFilmographies.Assign(value); }
    }

    public Anime() {
      _animeFilmographies = new EntitySet<AnimeFilmography>();
    }

    public Anime(string title, string title_kana, int year, string url) : this() {
      Title = title;
      TitleKana = title_kana;
      Year = year;
      Url = url;
    }

    public void Replace(Anime entity) {
      Title = entity.Title;
      Title = entity.TitleKana;
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
      return $"Id: {Id}, Title: {Title}, TitleKana: {TitleKana}, Year: {Year}, Url: {Url ?? "NULL"}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
