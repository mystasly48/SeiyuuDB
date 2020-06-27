﻿using Newtonsoft.Json;
using System;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Entities {
  [Table(Name = "GameFilmography")]
  [JsonObject("GameFilmography")]
  public sealed class GameFilmography : ISeiyuuEntity<GameFilmography> {
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

    [Column(Name = "game_id", CanBeNull = false, DbType = "INT")]
    [JsonProperty("game_id")]
    public int GameId { get; private set; }

    [JsonIgnore]
    public Game Game { get; private set; }

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

    public void Replace(GameFilmography entity) {
      Role = entity.Role;
      IsMainRole = entity.IsMainRole;
      ActorId = entity.ActorId;
      Actor = entity.Actor;
      GameId = entity.Game.Id;
      Game = entity.Game;
    }

    public bool IsReadyEntity() {
      return IsReadyEntityWithoutId() && Id != -1;
    }

    public bool IsReadyEntityWithoutId() {
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