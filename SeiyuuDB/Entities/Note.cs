using Newtonsoft.Json;
using System;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Entities {
  [Table(Name = "Note")]
  [JsonObject("Note")]
  public sealed class Note : SeiyuuBaseEntity<Note> {
    [Column(Name = "title", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    [JsonProperty("title")]
    public string Title { get; private set; }

    [Column(Name = "content", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    [JsonProperty("content")]
    public string Content { get; private set; }

    [Column(Name = "actor_id", CanBeNull = false, DbType = "INT")]
    [JsonProperty("actor_id")]
    public int ActorId { get; private set; }

    [JsonIgnore]
    public Actor Actor { get; private set; }

    public Note() { }

    public Note(string title, Actor actor, DateTime created_at, DateTime updated_at) {
      Title = title;
      ActorId = actor.Id;
      Actor = actor;
      CreatedAt = created_at;
      UpdatedAt = updated_at;
    }

    public Note(string title, string content, Actor actor, DateTime created_at, DateTime updated_at) {
      Title = title;
      Content = content;
      ActorId = actor.Id;
      Actor = actor;
      CreatedAt = created_at;
      UpdatedAt = updated_at;
    }

    public Note(Note note, Actor actor) : this(note.Title, note.Content, actor, note.CreatedAt, note.UpdatedAt) {
      Id = note.Id;
    }

    public override void Replace(Note entity) {
      Title = entity.Title;
      Content = entity.Content;
      ActorId = entity.ActorId;
      Actor = entity.Actor;
    }

    public override bool IsReadyEntity() {
      return IsReadyEntityWithoutId() && Id != -1;
    }

    public override bool IsReadyEntityWithoutId() {
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
