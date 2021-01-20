using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Entities {
  /// <summary>
  /// 外部リンク
  /// </summary>
  [Table(Name = "external_links")]
  public sealed class ExternalLink : ISeiyuuEntity<ExternalLink> {
    /// <summary>
    /// 外部リンクID
    /// </summary>
    [Column(Name = "id", CanBeNull = false, DbType = "INT", IsPrimaryKey = true)]
    public int Id { get; set; } = -1;

    /// <summary>
    /// タイトル
    /// </summary>
    [Column(Name = "title", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    public string Title { get; private set; }

    /// <summary>
    /// URL
    /// </summary>
    [Column(Name = "url", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    public string Url { get; private set; }

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

    public ExternalLink() { }
    public ExternalLink(string title, Actor actor, string url) {
      Title = title;
      ActorId = actor.Id;
      Actor = actor;
      Url = url;
    }
    
    public void Replace(ExternalLink entity) {
      Title = entity.Title;
      ActorId = entity.ActorId;
      Actor = entity.Actor;
      Url = entity.Url;
    }

    public bool IsReadyEntity() {
      return IsReadyEntityWithoutId() && Id != -1;
    }

    public bool IsReadyEntityWithoutId() {
      return Url != null;
    }

    public override bool Equals(object obj) {
      if (obj is ExternalLink item) {
        return item.Url == Url && item.ActorId == ActorId;
      }
      return false;
    }

    public override int GetHashCode() {
      return Tuple.Create(Url, ActorId).GetHashCode();
    }

    public static bool operator ==(ExternalLink a, ExternalLink b) {
      return Equals(a, b);
    }

    public static bool operator !=(ExternalLink a, ExternalLink b) {
      return !Equals(a, b);
    }

    public override string ToString() {
      return $"Id: {Id}, Title: {Title ?? "NULL"}, Actor: ({Actor}), Url: {Url}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
