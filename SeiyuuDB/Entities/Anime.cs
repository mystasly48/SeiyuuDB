using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Entities {
  /// <summary>
  /// アニメ
  /// </summary>
  [Table(Name = "animes")]
  public sealed class Anime : ISeiyuuEntity<Anime> {
    /// <summary>
    /// アニメID
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
    /// 放送年
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
    /// アニメキャラクタ一覧
    /// </summary>
    [Association(OtherKey = "AnimeId", Storage = "_animesCharacters")]
    public EntitySet<AnimeCharacter> AnimesCharacters {
      get { return _animesCharacters; }
      set { _animesCharacters.Assign(value); }
    }
    private EntitySet<AnimeCharacter> _animesCharacters;

    public Anime() {
      _animesCharacters = new EntitySet<AnimeCharacter>();
    }

    public Anime(string title, string titleKana, string alias, int releasedYear, string url) : this() {
      Title = title;
      TitleKana = titleKana;
      Alias = alias;
      ReleasedYear = releasedYear;
      Url = url;
    }

    public void Replace(Anime entity) {
      Title = entity.Title;
      Title = entity.TitleKana;
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
      return $"Id: {Id}, Title: {Title}, TitleKana: {TitleKana}, Alias: {Alias}, ReleasedYear: {ReleasedYear}, Url: {Url ?? "NULL"}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
