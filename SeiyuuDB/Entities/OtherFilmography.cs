using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeiyuuDB.Entities {
  [Table(Name = "OtherFilmography")]
  [JsonObject("OtherFilmography")]
  public sealed class OtherFilmography : ISeiyuuEntity<OtherFilmography> {
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

    [Column(Name = "role", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    [JsonProperty("role")]
    public string Role { get; private set; }

    [Column(Name = "is_main_role", CanBeNull = false, DbType = "INT")]
    [JsonProperty("is_main_role")]
    private int _isMainRole;

    [JsonIgnore]
    public bool IsMainRole {
      get {
        return _isMainRole == 1;
      }
      private set {
        _isMainRole = value ? 1 : 0;
      }
    }

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

    // TODO Date の対応

    [Column(Name = "date", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    [JsonProperty("date")]
    public string _date;

    [JsonIgnore]
    public DateTime? Date {
      get {
        if (_date is null) {
          return null;
        } else {
          return (DateTime?)DateTime.Parse(_date);
        }
      }
      private set {
        if (value.HasValue) {
          _date = value.Value.ToString();
        } else {
          _date = null;
        }
      }
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

    public OtherFilmography() { }
    public OtherFilmography(string title, string role, bool is_main_role, Actor actor, DateTime date) {
      Title = title;
      Role = role;
      IsMainRole = is_main_role;
      ActorId = actor.Id;
      Actor = actor;
      Date = date;
    }

    public void Replace(OtherFilmography entity) {
      Title = entity.Title;
      Role = entity.Role;
      IsMainRole = entity.IsMainRole;
      ActorId = entity.Actor.Id;
      Actor = entity.Actor;
      Date = entity.Date;
    }

    public bool IsReadyEntity() {
      return IsReadyEntityWithoutId() && Id != -1;
    }

    public bool IsReadyEntityWithoutId() {
      return Role != null;
    }

    public override bool Equals(object obj) {
      if (obj is OtherFilmography item) {
        return item.Title == Title && item.Role == Role && item.ActorId == ActorId;
      }
      return false;
    }

    public override int GetHashCode() {
      return Tuple.Create(Title, Role, ActorId).GetHashCode();
    }

    public static bool operator ==(OtherFilmography a, OtherFilmography b) {
      return Equals(a, b);
    }

    public static bool operator !=(OtherFilmography a, OtherFilmography b) {
      return !Equals(a, b);
    }

    public override string ToString() {
      return $"Id: {Id}, Title: {Title}, Role: {Role}, IsMainRole: {IsMainRole}, Actor: ({Actor}), Date: {Date}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
