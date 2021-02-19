using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Core.Entities {
  /// <summary>
  /// ゲームキャラクタ
  /// </summary>
  [Table(Name = "games_characters")]
  public sealed class GameCharacter : ISeiyuuEntity<GameCharacter> {
    /// <summary>
    /// ゲームキャラクタID
    /// </summary>
    [Column(Name = "id", CanBeNull = false, DbType = "INT", IsPrimaryKey = true)]
    public int Id { get; set; } = -1;

    /// <summary>
    /// キャラクタID
    /// </summary>
    [Column(Name = "character_id", CanBeNull = false, DbType = "INT")]
    public int CharacterId { get; private set; }

    /// <summary>
    /// キャラクタ
    /// </summary>
    [Association(Storage = "_character", ThisKey = "CharacterId", IsForeignKey = true)]
    public Character Character {
      get { return _character.Entity; }
      private set { _character.Entity = value; }
    }
    private EntityRef<Character> _character;

    /// <summary>
    /// ゲームID
    /// </summary>
    [Column(Name = "game_id", CanBeNull = false, DbType = "INT")]
    public int GameId { get; private set; }

    /// <summary>
    /// ゲーム
    /// </summary>
    [Association(Storage = "_game", ThisKey = "GameId", IsForeignKey = true)]
    public Game Game {
      get { return _game.Entity; }
      private set { _game.Entity = value; }
    }
    private EntityRef<Game> _game;

    [Column(Name = "created_at", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    private string _createdAt;

    /// <summary>
    /// 作成日時
    /// </summary>
    public DateTime CreatedAt {
      get { return DateTime.Parse(_createdAt); }
      set { _createdAt = value.ToString(); }
    }

    [Column(Name = "updated_at", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    private string _updatedAt;

    /// <summary>
    /// 更新日時
    /// </summary>
    public DateTime UpdatedAt {
      get { return DateTime.Parse(_updatedAt); }
      set { _updatedAt = value.ToString(); }
    }

    public GameCharacter() { }
    public GameCharacter(Character character, Game game) {
      CharacterId = character.Id;
      Character = character;
      GameId = game.Id;
      Game = game;
    }

    public void Replace(GameCharacter entity) {
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
      if (obj is GameCharacter item) {
        return item.CharacterId == CharacterId && item.GameId == GameId;
      }
      return false;
    }

    public override int GetHashCode() {
      return Tuple.Create(CharacterId, GameId).GetHashCode();
    }

    public static bool operator ==(GameCharacter a, GameCharacter b) {
      return Equals(a, b);
    }

    public static bool operator !=(GameCharacter a, GameCharacter b) {
      return !Equals(a, b);
    }

    public override string ToString() {
      return $"Id: {Id}, Character: ({Character}), Game: ({Game}), CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
