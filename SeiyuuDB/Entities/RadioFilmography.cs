using Newtonsoft.Json;
using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Entities {
  [Table(Name = "RadioFilmography")]
  [JsonObject("RadioFilmography")]
  public sealed class RadioFilmography : ISeiyuuEntity<RadioFilmography> {
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

    [Column(Name = "actor_id", CanBeNull = false, DbType = "INT")]
    [JsonProperty("actor_id")]
    public int ActorId { get; private set; }

    private EntityRef<Actor> _actor;
    [Association(Storage = "_actor", ThisKey = "ActorId", IsForeignKey = true)]
    [JsonIgnore]
    public Actor Actor {
      get { return _actor.Entity; }
      private set { _actor.Entity = value; }
    }

    [Column(Name = "radio_id", CanBeNull = false, DbType = "INT")]
    [JsonProperty("radio_id")]
    public int RadioId { get; private set; }

    private EntityRef<Radio> _radio;
    [Association(Storage = "_radio", ThisKey = "RadioId", IsForeignKey = true)]
    [JsonIgnore]
    public Radio Radio {
      get { return _radio.Entity; }
      private set { _radio.Entity = value; }
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

    public RadioFilmography() { }
    public RadioFilmography(Actor actor, Radio radio) {
      ActorId = actor.Id;
      Actor = actor;
      RadioId = radio.Id;
      Radio = radio;
    }

    public void Replace(RadioFilmography entity) {
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
      if (obj is RadioFilmography item) {
        return item.ActorId == ActorId && item.RadioId == RadioId;
      }
      return false;
    }

    public override int GetHashCode() {
      return Tuple.Create(ActorId, RadioId).GetHashCode();
    }

    public static bool operator ==(RadioFilmography a, RadioFilmography b) {
      return Equals(a, b);
    }

    public static bool operator !=(RadioFilmography a, RadioFilmography b) {
      return !Equals(a, b);
    }

    public override string ToString() {
      return $"Id: {Id}, Actor: ({Actor}), Radio: ({Radio}), CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
