﻿using Newtonsoft.Json;
using SeiyuuDB.Helpers;
using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Entities {
  [Table(Name = "ExternalLink")]
  [JsonObject("ExternalLink")]
  public sealed class ExternalLink : ISeiyuuEntity<ExternalLink> {
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

    [Column(Name = "title", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    [JsonProperty("title")]
    public string Title { get; private set; }

    [Column(Name = "url", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    [JsonProperty("url")]
    public string Url { get; private set; }

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
    
    public bool Contains(string value) {
      return Title.ContainsOriginally(value);
    }
  }
}
