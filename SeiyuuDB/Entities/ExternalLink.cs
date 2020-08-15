using Newtonsoft.Json;
using System;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Entities {
  [Table(Name = "ExternalLink")]
  [JsonObject("ExternalLink")]
  public sealed class ExternalLink : SeiyuuBaseEntity<ExternalLink> {
    [Column(Name = "title", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    [JsonProperty("title")]
    public string Title { get; private set; }

    [Column(Name = "url", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    [JsonProperty("url")]
    public string Url { get; private set; }

    [Column(Name = "actor_id", CanBeNull = false, DbType = "INT")]
    [JsonProperty("actor_id")]
    public int ActorId { get; private set; }

    [JsonIgnore]
    public Actor Actor { get; private set; }

    public ExternalLink() { }

    public ExternalLink(string title, Actor actor, string url, DateTime created_at, DateTime updated_at) {
      Title = title;
      ActorId = actor.Id;
      Actor = actor;
      Url = url;
      CreatedAt = created_at;
      UpdatedAt = updated_at;
    }

    public ExternalLink(ExternalLink link, Actor actor) : this(link.Title, actor, link.Url, link.CreatedAt, link.UpdatedAt) {
      Id = link.Id;
    }

    public override void Replace(ExternalLink entity) {
      Title = entity.Title;
      ActorId = entity.ActorId;
      Actor = entity.Actor;
      Url = entity.Url;
    }

    public override bool IsReadyEntity() {
      return IsReadyEntityWithoutId() && Id != -1;
    }

    public override bool IsReadyEntityWithoutId() {
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
