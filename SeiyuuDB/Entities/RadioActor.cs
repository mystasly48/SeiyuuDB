using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Entities {
  /// <summary>
  /// ラジオ声優
  /// </summary>
  [Table(Name = "radios_actors")]
  public sealed class RadioActor : ISeiyuuEntity<RadioActor> {
    /// <summary>
    /// ラジオ声優ID
    /// </summary>
    [Column(Name = "id", CanBeNull = false, DbType = "INT", IsPrimaryKey = true)]
    public int Id { get; set; } = -1;

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
    /// ラジオID
    /// </summary>
    [Column(Name = "radio_id", CanBeNull = false, DbType = "INT")]
    public int RadioId { get; private set; }

    /// <summary>
    /// ラジオ
    /// </summary>
    [Association(Storage = "_radio", ThisKey = "RadioId", IsForeignKey = true)]
    public Radio Radio {
      get { return _radio.Entity; }
      private set { _radio.Entity = value; }
    }
    private EntityRef<Radio> _radio;

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

    public RadioActor() { }
    public RadioActor(Actor actor, Radio radio) {
      ActorId = actor.Id;
      Actor = actor;
      RadioId = radio.Id;
      Radio = radio;
    }

    public void Replace(RadioActor entity) {
      ActorId = entity.ActorId;
      Actor = entity.Actor;
      RadioId = entity.RadioId;
      Radio = entity.Radio;
    }

    public bool IsReadyEntity() {
      return IsReadyEntityWithoutId() && Id != -1;
    }

    public bool IsReadyEntityWithoutId() {
      return true;
    }

    public override bool Equals(object obj) {
      if (obj is RadioActor item) {
        return item.ActorId == ActorId && item.RadioId == RadioId;
      }
      return false;
    }

    public override int GetHashCode() {
      return Tuple.Create(ActorId, RadioId).GetHashCode();
    }

    public static bool operator ==(RadioActor a, RadioActor b) {
      return Equals(a, b);
    }

    public static bool operator !=(RadioActor a, RadioActor b) {
      return !Equals(a, b);
    }

    public override string ToString() {
      return $"Id: {Id}, Actor: ({Actor}), Radio: ({Radio}), CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
