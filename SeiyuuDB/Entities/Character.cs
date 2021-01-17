using Newtonsoft.Json;
using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Entities {
  [Table(Name = "Character")]
  [JsonObject("Character")]
  public sealed class Character : ISeiyuuEntity<Character> {
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

    [Column(Name = "name", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    [JsonProperty("name")]
    public string Name { get; private set; }

    [Column(Name = "name_kana", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    [JsonProperty("name_kana")]
    public string NameKana { get; private set; }

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

    [Column(Name = "picture_url", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    [JsonProperty("picture_url")]
    public string PictureUrl { get; private set; }

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

    private EntitySet<AnimeFilmography> _animeFilmographies;
    [Association(OtherKey = "CharacterId", Storage = "_animeFilmographies")]
    public EntitySet<AnimeFilmography> AnimeFilmographies {
      get { return _animeFilmographies; }
      set { _animeFilmographies.Assign(value); }
    }

    private EntitySet<GameFilmography> _gameFilmographies;
    [Association(OtherKey = "CharacterId", Storage = "_gameFilmographies")]
    public EntitySet<GameFilmography> GameFilmographies {
      get { return _gameFilmographies; }
      set { _gameFilmographies.Assign(value); }
    }

    public Character() {
      _animeFilmographies = new EntitySet<AnimeFilmography>();
      _gameFilmographies = new EntitySet<GameFilmography>();
    }

    public Character(string name, string name_kana, bool is_main_role, string picture_url, Actor actor) : this() {
      Name = name;
      NameKana = name_kana;
      IsMainRole = is_main_role;
      PictureUrl = picture_url;
      ActorId = actor.Id;
      Actor = actor;
    }

    public void Replace(Character entity) {
      Name = entity.Name;
      NameKana = entity.NameKana;
      PictureUrl = entity.PictureUrl;
      IsMainRole = entity.IsMainRole;
      ActorId = entity.ActorId;
      Actor = entity.Actor;
    }

    public bool IsReadyEntity() {
      return IsReadyEntityWithoutId() && Id != -1;
    }

    public bool IsReadyEntityWithoutId() {
      return Name != null && Actor != null;
    }

    public override bool Equals(object obj) {
      if (obj is Character item) {
        return item.Name == Name && item.ActorId == ActorId;
      }
      return false;
    }

    public override int GetHashCode() {
      return Tuple.Create(Name, ActorId).GetHashCode();
    }

    public static bool operator ==(Character a, Character b) {
      return Equals(a, b);
    }

    public static bool operator !=(Character a, Character b) {
      return !Equals(a, b);
    }

    public override string ToString() {
      return $"Id: {Id}, Name: {Name}, NameKana: {NameKana}, IsMainRole: {IsMainRole}, PictureUrl: {PictureUrl}, Actor: ({Actor}), CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
