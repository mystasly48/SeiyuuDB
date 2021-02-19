using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Core.Entities {
  /// <summary>
  /// アニメキャラクタ
  /// </summary>
  [Table(Name = "animes_characters")]
  public sealed class AnimeCharacter : ISeiyuuEntity<AnimeCharacter> {
    /// <summary>
    /// アニメキャラクタID
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
    /// アニメID
    /// </summary>
    [Column(Name = "anime_id", CanBeNull = false, DbType = "INT")]
    public int AnimeId { get; private set; }

    /// <summary>
    /// アニメ
    /// </summary>
    [Association(Storage = "_anime", ThisKey = "AnimeId", IsForeignKey = true)]
    public Anime Anime {
      get { return _anime.Entity; }
      private set { _anime.Entity = value; }
    }
    private EntityRef<Anime> _anime;

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

    public AnimeCharacter() { }
    public AnimeCharacter(Character character, Anime anime) {
      CharacterId = character.Id;
      Character = character;
      AnimeId = anime.Id;
      Anime = anime;
    }

    public void Replace(AnimeCharacter entity) {
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
      if (obj is AnimeCharacter item) {
        return item.CharacterId == CharacterId && item.AnimeId == AnimeId;
      }
      return false;
    }

    public override int GetHashCode() {
      return Tuple.Create(CharacterId, AnimeId).GetHashCode();
    }

    public static bool operator ==(AnimeCharacter a, AnimeCharacter b) {
      return Equals(a, b);
    }

    public static bool operator !=(AnimeCharacter a, AnimeCharacter b) {
      return !Equals(a, b);
    }

    public override string ToString() {
      return $"Id: {Id}, Character: ({Character}), Anime: ({Anime}), CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
