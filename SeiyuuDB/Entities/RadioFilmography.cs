using Newtonsoft.Json;
using System;
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

    [JsonIgnore]
    public Actor Actor { get; private set; }

    [Column(Name = "radio_id", CanBeNull = false, DbType = "INT")]
    [JsonProperty("radio_id")]
    public int RadioId { get; private set; }

    [JsonIgnore]
    public Radio Radio { get; private set; }

    [Column(Name = "created_at", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    [JsonProperty("created_at")]
    private string _createdAt;

    [JsonIgnore]
    public DateTime CreatedAt {
      get {
        return DateTime.Parse(_createdAt);
      }
      private set {
        _createdAt = value.ToString();
      }
    }

    [Column(Name = "updated_at", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    [JsonProperty("updated_at")]
    private string _updatedAt;

    [JsonIgnore]
    public DateTime UpdatedAt {
      get {
        return DateTime.Parse(_updatedAt);
      }
      set {
        _updatedAt = value.ToString();
      }
    }

    public RadioFilmography() { }

    public RadioFilmography(Actor actor, Radio radio, DateTime created_at, DateTime updated_at) {
      ActorId = actor.Id;
      Actor = actor;
      RadioId = radio.Id;
      Radio = radio;
      CreatedAt = created_at;
      UpdatedAt = updated_at;
    }

    public RadioFilmography(RadioFilmography film, Actor actor, Radio radio) : this(actor, radio, film.CreatedAt, film.UpdatedAt) {
      Id = film.Id;
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
