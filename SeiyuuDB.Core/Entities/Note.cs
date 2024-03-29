﻿using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace SeiyuuDB.Core.Entities {
  /// <summary>
  /// ノート
  /// </summary>
  [Table(Name = "notes")]
  public sealed class Note : ISeiyuuEntity<Note> {
    /// <summary>
    /// ノートID
    /// </summary>
    [Column(Name = "id", CanBeNull = false, DbType = "INT", IsPrimaryKey = true)]
    public int Id { get; set; } = -1;

    /// <summary>
    /// タイトル
    /// </summary>
    [Column(Name = "title", CanBeNull = false, DbType = "VARCHAR(MAX)")]
    public string Title { get; private set; }

    /// <summary>
    /// 内容
    /// </summary>
    [Column(Name = "content", CanBeNull = true, DbType = "VARCHAR(MAX)")]
    public string Content { get; private set; }

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

    public Note() { }
    public Note(string title, string content, Actor actor) {
      Title = title;
      Content = content;
      ActorId = actor.Id;
      Actor = actor;
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
      return $"Id: {Id}, Title: {Title}, Content: {Content}, Actor: ({Actor}), "
        + "CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
  }
}
