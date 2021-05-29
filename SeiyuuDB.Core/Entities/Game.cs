using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Core.Entities {
  /// <summary>
  /// ゲーム
  /// </summary>
  [Table(Name = "games")]
  public sealed class Game : ISeiyuuEntity<Game> {
    /// <summary>
    /// ゲームID
    /// </summary>
    [Column(Name = "id", CanBeNull = false, DbType = "INT", IsPrimaryKey = true)]
    public int Id { get; set; } = -1;

    /// <summary>
    /// タイトル
    /// </summary>
    [Column(Name = "title", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    public string Title { get; private set; }

    /// <summary>
    /// タイトルかな
    /// </summary>
    [Column(Name = "title_kana", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    public string TitleKana { get; private set; }

    /// <summary>
    /// 別称
    /// </summary>
    [Column(Name = "alias", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    public string Alias { get; private set; }

    /// <summary>
    /// 発売年
    /// </summary>
    [Column(Name = "released_year", CanBeNull = false, DbType = "INT")]
    public int ReleasedYear { get; private set; }

    /// <summary>
    /// URL
    /// </summary>
    [Column(Name = "url", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    public string Url { get; private set; }

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
    /// ゲームキャラクタ一覧
    /// </summary>
    [Association(OtherKey = "GameId", Storage = "_gamesCharacters")]
    public EntitySet<GameCharacter> GamesCharacters {
      get { return _gamesCharacters; }
      set { _gamesCharacters.Assign(value); }
    }
    private EntitySet<GameCharacter> _gamesCharacters;

    public Game() {
      _gamesCharacters = new EntitySet<GameCharacter>();
    }

    public Game(string title, string titleKana, string alias, int releasedYear, string url) : this() {
      Title = title;
      TitleKana = titleKana;
      Alias = alias;
      ReleasedYear = releasedYear;
      Url = url;
    }

    public void Replace(Game entity) {
      Title = entity.Title;
      TitleKana = entity.TitleKana;
      Alias = entity.Alias;
      ReleasedYear = entity.ReleasedYear;
      Url = entity.Url;
    }

    public bool IsReadyEntity() {
      return IsReadyEntityWithoutId() && Id != -1;
    }

    public bool IsReadyEntityWithoutId() {
      return Title != null;
    }

    public override bool Equals(object obj) {
      if (obj is Game item) {
        return item.Title == Title;
      }
      return false;
    }

    public override int GetHashCode() {
      return Title.GetHashCode();
    }

    public static bool operator ==(Game a, Game b) {
      return Equals(a, b);
    }

    public static bool operator !=(Game a, Game b) {
      return !Equals(a, b);
    }

    public override string ToString() {
      return $"Id: {Id}, Title: {Title}, TitleKana: {TitleKana}, Alias: {Alias}, "
        + $"ReleasedYear: {ReleasedYear}, Url: {Url ?? "NULL"}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
