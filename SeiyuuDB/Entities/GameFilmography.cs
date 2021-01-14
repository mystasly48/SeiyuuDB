using Newtonsoft.Json;
using System;
using System.Data.Linq;
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

    [Column(Name = "game_id", CanBeNull = false, DbType = "INT")]
    [JsonProperty("game_id")]
    public int GameId { get; private set; }

    private EntityRef<Game> _game;
    [Association(Storage = "_game", ThisKey = "GameId", IsForeignKey = true)]
    [JsonIgnore]
    public Game Game {
      get { return _game.Entity; }
      private set { _game.Entity = value; }
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

    public GameFilmography() { }
    public GameFilmography(Character character, Game game) {
      CharacterId = character.Id;
      Character = character;
      GameId = game.Id;
      Game = game;
    }

    public void Replace(GameFilmography entity) {
      CharacterId = entity.CharacterId;
      Character = entity.Character;
      GameId = entity.Game.Id;
      Game = entity.Game;
    }

    public bool IsReadyEntity() {
      return IsReadyEntityWithoutId() && Id != -1;
    }

    public bool IsReadyEntityWithoutId() {
      return true;
    }

    public override bool Equals(object obj) {
      if (obj is GameFilmography item) {
        return item.CharacterId == CharacterId && item.GameId == GameId;
      }
      return false;
    }

    public override int GetHashCode() {
      return Tuple.Create(CharacterId, GameId).GetHashCode();
    }

    public static bool operator ==(GameFilmography a, GameFilmography b) {
      return Equals(a, b);
    }

    public static bool operator !=(GameFilmography a, GameFilmography b) {
      return !Equals(a, b);
    }

    public override string ToString() {
      return $"Id: {Id}, Character: ({Character}), Game: ({Game}), CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
