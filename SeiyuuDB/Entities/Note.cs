using Newtonsoft.Json;
using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Entities {
  [Table(Name = "Note")]
  [JsonObject("Note")]
  public sealed class Note : ISeiyuuEntity<Note> {
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

    [Column(Name = "title", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    [JsonProperty("title")]
    public string Title { get; private set; }

    [Column(Name = "content", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    [JsonProperty("content")]
    public string Content { get; private set; }

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

    public Note() { }
    public Note(string title, string content, Actor actor, DateTime created_at, DateTime updated_at) {
      Title = title;
      Content = content;
      ActorId = actor.Id;
      Actor = actor;
      CreatedAt = created_at;
      UpdatedAt = updated_at;
    }

    public void Replace(Note entity) {
      Title = entity.Title;
      Content = entity.Content;
      ActorId = entity.ActorId;
      Actor = entity.Actor;
    }

    public bool IsReadyEntity() {
      return IsReadyEntityWithoutId() && Id != -1;
    }

    public bool IsReadyEntityWithoutId() {
      return Title != null;
    }

    public override bool Equals(object obj) {
      if (obj is Note item) {
        return item.Title == Title && item.ActorId == ActorId;
      }
      return false;
    }

    public override int GetHashCode() {
      return Tuple.Create(Title, ActorId).GetHashCode();
    }

    public static bool operator ==(Note a, Note b) {
      return Equals(a, b);
    }

    public static bool operator !=(Note a, Note b) {
      return !Equals(a, b);
    }

    public override string ToString() {
      return $"Id: {Id}, Title: {Title}, Content: {Content ?? "NULL"}, Actor: ({Actor}), CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
