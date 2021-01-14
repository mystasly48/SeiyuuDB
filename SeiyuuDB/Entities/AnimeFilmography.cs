﻿using Newtonsoft.Json;
using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Entities {
  [Table(Name = "AnimeFilmography")]
  [JsonObject("AnimeFilmography")]
  public sealed class AnimeFilmography : ISeiyuuEntity<AnimeFilmography> {
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

    [Column(Name = "character_id", CanBeNull = false, DbType = "INT")]
    [JsonProperty("character_id")]
    public int CharacterId { get; private set; }

    private EntityRef<Character> _character;
    [Association(Storage = "_character", ThisKey = "CharacterId", IsForeignKey = true)]
    [JsonIgnore]
    public Character Character {
      get { return _character.Entity; }
      private set { _character.Entity = value; }
    }

    [Column(Name = "anime_id", CanBeNull = false, DbType = "INT")]
    [JsonProperty("anime_id")]
    public int AnimeId { get; private set; }

    private EntityRef<Anime> _anime;
    [Association(Storage = "_anime", ThisKey = "AnimeId", IsForeignKey = true)]
    [JsonIgnore]
    public Anime Anime {
      get { return _anime.Entity; }
      private set { _anime.Entity = value; }
    }

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

    public AnimeFilmography() { }
    public AnimeFilmography(Character character, Anime anime) {
      CharacterId = character.Id;
      Character = character;
      AnimeId = anime.Id;
      Anime = anime;
    }

    public void Replace(AnimeFilmography entity) {
      CharacterId = entity.CharacterId;
      Character = entity.Character;
      AnimeId = entity.Anime.Id;
      Anime = entity.Anime;
    }

    public bool IsReadyEntity() {
      return IsReadyEntityWithoutId() && Id != -1;
    }

    public bool IsReadyEntityWithoutId() {
      return true;
    }

    public override bool Equals(object obj) {
      if (obj is AnimeFilmography item) {
        return item.CharacterId == CharacterId && item.AnimeId == AnimeId;
      }
      return false;
    }

    public override int GetHashCode() {
      return Tuple.Create(CharacterId, AnimeId).GetHashCode();
    }

    public static bool operator ==(AnimeFilmography a, AnimeFilmography b) {
      return Equals(a, b);
    }

    public static bool operator !=(AnimeFilmography a, AnimeFilmography b) {
      return !Equals(a, b);
    }

    public override string ToString() {
      return $"Id: {Id}, Character: ({Character}), Anime: ({Anime}), CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
