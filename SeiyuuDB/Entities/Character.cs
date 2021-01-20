using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Entities {
  /// <summary>
  /// キャラクタ
  /// </summary>
  [Table(Name = "characters")]
  public sealed class Character : ISeiyuuEntity<Character> {
    /// <summary>
    /// キャラクタID
    /// </summary>
    [Column(Name = "id", CanBeNull = false, DbType = "INT", IsPrimaryKey = true)]
    public int Id { get; set; } = -1;

    /// <summary>
    /// 名前
    /// </summary>
    [Column(Name = "name", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    public string Name { get; private set; }

    /// <summary>
    /// 名前かな
    /// </summary>
    [Column(Name = "name_kana", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    public string NameKana { get; private set; }

    [Column(Name = "is_main_role", CanBeNull = false, DbType = "INT")]
    private int _isMainRole;

    /// <summary>
    /// 主役フラグ
    /// </summary>
    public bool IsMainRole {
      get {
        return _isMainRole == 1;
      }
      private set {
        _isMainRole = value ? 1 : 0;
      }
    }

    /// <summary>
    /// 声優ID
    /// </summary>
    [Column(Name = "actor_id", CanBeNull = false, DbType = "INT")]
    public int ActorId { get; private set; }

    /// <summary>
    /// 声優
    /// </summary>
    [Association(Storage = "_actor", ThisKey = "ActorId", IsForeignKey = true)]
    public Actor Actor {
      get { return _actor.Entity; }
      private set { _actor.Entity = value; }
    }
    private EntityRef<Actor> _actor;

    /// <summary>
    /// 写真URL
    /// </summary>
    [Column(Name = "picture_url", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    public string PictureUrl { get; private set; }

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

    /// <summary>
    /// アニメキャラクタ一覧
    /// </summary>
    [Association(OtherKey = "CharacterId", Storage = "_animesCharacters")]
    public EntitySet<AnimeCharacter> AnimesCharacters {
      get { return _animesCharacters; }
      set { _animesCharacters.Assign(value); }
    }
    private EntitySet<AnimeCharacter> _animesCharacters;

    /// <summary>
    /// ゲームキャラクタ一覧
    /// </summary>
    [Association(OtherKey = "CharacterId", Storage = "_gamesCharacters")]
    public EntitySet<GameCharacter> GamesCharacters {
      get { return _gamesCharacters; }
      set { _gamesCharacters.Assign(value); }
    }
    private EntitySet<GameCharacter> _gamesCharacters;

    public Character() {
      _animesCharacters = new EntitySet<AnimeCharacter>();
      _gamesCharacters = new EntitySet<GameCharacter>();
    }

    public Character(string name, string nameKana, bool isMainRole, string pictureUrl, Actor actor) : this() {
      Name = name;
      NameKana = nameKana;
      IsMainRole = isMainRole;
      PictureUrl = pictureUrl;
      ActorId = actor.Id;
      Actor = actor;
    }

    public void Replace(Character entity) {
      Name = entity.Name;
      NameKana = entity.NameKana;
      PictureUrl = entity.PictureUrl;
      IsMainRole = entity.IsMainRole;
      ActorId = entity.ActorId;
      Actor = entity.Actor;
    }

    public bool IsReadyEntity() {
      return IsReadyEntityWithoutId() && Id != -1;
    }

    public bool IsReadyEntityWithoutId() {
      return Name != null && Actor != null;
    }

    public override bool Equals(object obj) {
      if (obj is Character item) {
        return item.Name == Name && item.ActorId == ActorId;
      }
      return false;
    }

    public override int GetHashCode() {
      return Tuple.Create(Name, ActorId).GetHashCode();
    }

    public static bool operator ==(Character a, Character b) {
      return Equals(a, b);
    }

    public static bool operator !=(Character a, Character b) {
      return !Equals(a, b);
    }

    public override string ToString() {
      return $"Id: {Id}, Name: {Name}, NameKana: {NameKana}, IsMainRole: {IsMainRole}, PictureUrl: {PictureUrl}, Actor: ({Actor}), CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
