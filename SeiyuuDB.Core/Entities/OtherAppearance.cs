using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Core.Entities {
  /// <summary>
  /// その他出演
  /// </summary>
  [Table(Name = "other_appearances")]
  public sealed class OtherAppearance : ISeiyuuEntity<OtherAppearance> {
    /// <summary>
    /// その他出演ID
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
    /// 役割
    /// </summary>
    [Column(Name = "role", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    public string Role { get; private set; }

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

    // TODO Date の対応
    [Column(Name = "appeared_on", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    public string _appearedOn;

    /// <summary>
    /// 出演日
    /// </summary>
    public DateTime? AppearedOn {
      get {
        if (_appearedOn is null) {
          return null;
        } else {
          return (DateTime?)DateTime.Parse(_appearedOn);
        }
      }
      private set {
        if (value.HasValue) {
          _appearedOn = value.Value.ToString();
        } else {
          _appearedOn = null;
        }
      }
    }

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

    public OtherAppearance() { }
    public OtherAppearance(string title, string titleKana, string alias, string role, bool isMainRole, Actor actor, DateTime? appearedOn) {
      Title = title;
      TitleKana = titleKana;
      Alias = alias;
      Role = role;
      IsMainRole = isMainRole;
      AppearedOn = appearedOn;
      ActorId = actor.Id;
      Actor = actor;
      AppearedOn = appearedOn;
    }

    public void Replace(OtherAppearance entity) {
      Title = entity.Title;
      TitleKana = entity.TitleKana;
      Alias = entity.Alias;
      Role = entity.Role;
      IsMainRole = entity.IsMainRole;
      ActorId = entity.Actor.Id;
      Actor = entity.Actor;
      AppearedOn = entity.AppearedOn;
    }

    public bool IsReadyEntity() {
      return IsReadyEntityWithoutId() && Id != -1;
    }

    public bool IsReadyEntityWithoutId() {
      return Role != null;
    }

    public override bool Equals(object obj) {
      if (obj is OtherAppearance item) {
        return item.Title == Title && item.Role == Role && item.ActorId == ActorId;
      }
      return false;
    }

    public override int GetHashCode() {
      return Tuple.Create(Title, Role, ActorId).GetHashCode();
    }

    public static bool operator ==(OtherAppearance a, OtherAppearance b) {
      return Equals(a, b);
    }

    public static bool operator !=(OtherAppearance a, OtherAppearance b) {
      return !Equals(a, b);
    }

    public override string ToString() {
      return $"Id: {Id}, Title: {Title}, TitleKana: {TitleKana}, Alias: {Alias}, Role: {Role}, IsMainRole: {IsMainRole}, Actor: ({Actor}), AppearedOn: {AppearedOn}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
