using Newtonsoft.Json;
using System;
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

    [Column(Name = "role", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    [JsonProperty("role")]
    public string Role { get; private set; }

    [Column(Name = "is_main_role", CanBeNull = false, DbType = "INT")]
    [JsonProperty("is_main_role")]
    private int _isMainRole;

    [JsonIgnore]
    public bool IsMainRole {
      get {
        return _isMainRole == 1;
      }
      set {
        if (value) {
          _isMainRole = 1;
        } else {
          _isMainRole = 0;
        }
      }
    }

    [Column(Name = "actor_id", CanBeNull = false, DbType = "INT")]
    [JsonProperty("actor_id")]
    public int ActorId { get; private set; }

    [JsonIgnore]
    public Actor Actor { get; private set; }

    [Column(Name = "anime_id", CanBeNull = false, DbType = "INT")]
    [JsonProperty("anime_id")]
    public int AnimeId { get; private set; }

    [JsonIgnore]
    public Anime Anime { get; private set; }

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

    public AnimeFilmography() { }

    public AnimeFilmography(string role, bool is_main_role, Actor actor, Anime anime, DateTime created_at, DateTime updated_at) {
      Role = role;
      IsMainRole = is_main_role;
      ActorId = actor.Id;
      Actor = actor;
      AnimeId = anime.Id;
      Anime = anime;
      CreatedAt = created_at;
      UpdatedAt = updated_at;
    }

    public AnimeFilmography(AnimeFilmography film, Actor actor, Anime anime) : this(film.Role, film.IsMainRole, actor, anime, film.CreatedAt, film.UpdatedAt) {
      Id = film.Id;
    }

    public void Replace(AnimeFilmography entity) {
      Role = entity.Role;
      IsMainRole = entity.IsMainRole;
      ActorId = entity.ActorId;
      Actor = entity.Actor;
      AnimeId = entity.Anime.Id;
      Anime = entity.Anime;
    }

    public bool IsReadyEntity() {
      return IsReadyEntityWithoutId() && Id != -1;
    }

    public bool IsReadyEntityWithoutId() {
      return Role != null;
    }

    public override bool Equals(object obj) {
      if (obj is AnimeFilmography item) {
        return item.Role == Role && item.ActorId == ActorId && item.AnimeId == AnimeId;
      }
      return false;
    }

    public override int GetHashCode() {
      return Tuple.Create(Role, ActorId, AnimeId).GetHashCode();
    }

    public static bool operator ==(AnimeFilmography a, AnimeFilmography b) {
      return Equals(a, b);
    }

    public static bool operator !=(AnimeFilmography a, AnimeFilmography b) {
      return !Equals(a, b);
    }

    public override string ToString() {
      return $"Id: {Id}, Role: {Role}, IsMainRole: {IsMainRole}, Actor: ({Actor}), Anime: ({Anime}), CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}