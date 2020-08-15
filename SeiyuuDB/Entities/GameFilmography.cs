using Newtonsoft.Json;
using System;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Entities {
  [Table(Name = "GameFilmography")]
  [JsonObject("GameFilmography")]
  public sealed class GameFilmography : SeiyuuBaseEntity<GameFilmography> {
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

    [Column(Name = "game_id", CanBeNull = false, DbType = "INT")]
    [JsonProperty("game_id")]
    public int GameId { get; private set; }

    [JsonIgnore]
    public Game Game { get; private set; }

    public GameFilmography() { }

    public GameFilmography(string role, bool is_main_role, Actor actor, Game game, DateTime created_at, DateTime updated_at) {
      Role = role;
      IsMainRole = is_main_role;
      ActorId = actor.Id;
      Actor = actor;
      GameId = game.Id;
      Game = game;
      CreatedAt = created_at;
      UpdatedAt = updated_at;
    }

    public GameFilmography(GameFilmography film, Actor actor, Game game) : this(film.Role, film.IsMainRole, actor, game, film.CreatedAt, film.UpdatedAt) {
      Id = film.Id;
    }

    public override void Replace(GameFilmography entity) {
      Role = entity.Role;
      IsMainRole = entity.IsMainRole;
      ActorId = entity.ActorId;
      Actor = entity.Actor;
      GameId = entity.Game.Id;
      Game = entity.Game;
    }

    public override bool IsReadyEntity() {
      return IsReadyEntityWithoutId() && Id != -1;
    }

    public override bool IsReadyEntityWithoutId() {
      return Role != null;
    }

    public override bool Equals(object obj) {
      if (obj is GameFilmography item) {
        return item.Role == Role && item.ActorId == ActorId && item.GameId == GameId;
      }
      return false;
    }

    public override int GetHashCode() {
      return Tuple.Create(Role, ActorId, GameId).GetHashCode();
    }

    public static bool operator ==(GameFilmography a, GameFilmography b) {
      return Equals(a, b);
    }

    public static bool operator !=(GameFilmography a, GameFilmography b) {
      return !Equals(a, b);
    }

    public override string ToString() {
      return $"Id: {Id}, Role: {Role}, IsMainRole: {IsMainRole}, Actor: ({Actor}), Anime: ({Game}), CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}