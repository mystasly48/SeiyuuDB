using SeiyuuDB.Entities;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;

namespace SeiyuuDB.Databases {
  public class LocalSqlite : ISeiyuuDB, IDisposable {
    private SQLiteConnection _connection;
    private SQLiteCommand _command;
    private SeiyuuDataContext _context;

    public string DataSource { get; }
    public string BlobLocation { get; }

    public Actor[] Actors => _context.Actors.ToArray();
    public Anime[] Animes => _context.Animes.ToArray();
    public AnimeFilmography[] AnimeFilmographies => _context.AnimeFilmographies.ToArray();
    public Company[] Companies => _context.Companies.ToArray();
    public ExternalLink[] ExternalLinks => _context.ExternalLinks.ToArray();
    public Game[] Games => _context.Games.ToArray();
    public GameFilmography[] GameFilmographies => _context.GameFilmographies.ToArray();
    public Note[] Notes => _context.Notes.ToArray();
    public Radio[] Radios => _context.Radios.ToArray();
    public RadioFilmography[] RadioFilmographies => _context.RadioFilmographies.ToArray();

    public LocalSqlite(string dbSource, string blobLocation) {
      this.DataSource = dbSource;
      this.BlobLocation = blobLocation;
      var connStr = new SQLiteConnectionStringBuilder() { DataSource = this.DataSource };
      _connection = new SQLiteConnection(connStr.ToString());
      _connection.Open();
      _command = new SQLiteCommand(_connection);
      _context = new SeiyuuDataContext(_connection);
    }

    public void Dispose() {
      _context.Dispose();
      _command.Clone();
      _command.Dispose();
      _connection.Close();
      _connection.Dispose();
    }

    private int GetNextId<T>() where T : class, ISeiyuuEntity<T> {
      _command.CommandText = $"select id from {typeof(T).Name} order by rowid desc limit 1;";
      using (var reader = _command.ExecuteReader()) {
        while (reader.Read()) {
          return reader.GetInt32(0) + 1;
        }
      }
      return -1;
    }

    private Table<T> GetTable<T>() where T : class, ISeiyuuEntity<T> {
      return _context.GetTable<T>();
    }

    public T GetEntity<T>(int id) where T : class, ISeiyuuEntity<T> {
      object obj = null;
      if (typeof(T) == typeof(Actor)) {
        obj = _context.Actors.FirstOrDefault(x => x.Id == id);
      } else if (typeof(T) == typeof(Anime)) {
        obj = _context.Animes.FirstOrDefault(x => x.Id == id);
      } else if (typeof(T) == typeof(AnimeFilmography)) {
        obj = _context.AnimeFilmographies.FirstOrDefault(x => x.Id == id);
      } else if (typeof(T) == typeof(Game)) {
        obj = _context.Games.FirstOrDefault(x => x.Id == id);
      } else if (typeof(T) == typeof(GameFilmography)) {
        obj = _context.GameFilmographies.FirstOrDefault(x => x.Id == id);
      } else if (typeof(T) == typeof(Radio)) {
        obj = _context.Radios.FirstOrDefault(x => x.Id == id);
      } else if (typeof(T) == typeof(RadioFilmography)) {
        obj = _context.RadioFilmographies.FirstOrDefault(x => x.Id == id);
      } else if (typeof(T) == typeof(Company)) {
        obj = _context.Companies.FirstOrDefault(x => x.Id == id);
      } else if (typeof(T) == typeof(ExternalLink)) {
        obj = _context.ExternalLinks.FirstOrDefault(x => x.Id == id);
      } else if (typeof(T) == typeof(Note)) {
        obj  = _context.Notes.FirstOrDefault(x => x.Id == id);
      }
      return obj as T;
    }

    public T GetEntity<T>(T entity) where T : class, ISeiyuuEntity<T> {
      return GetTable<T>().FirstOrDefault(x => x.Equals(entity));
    }

    public bool IsExists<T>(T entity) where T : class, ISeiyuuEntity<T> {
      return GetTable<T>().Any(x => x.Equals(entity));
    }

    public int Insert<T>(T entity) where T : class, ISeiyuuEntity<T> {
      if (IsExists(entity) || !entity.IsReadyEntityWithoutId()) {
        return -1;
      }
      if (!entity.IsReadyEntity()) {
        entity.Id = GetNextId<T>();
      }
      entity.CreatedAt = DateTime.Now;
      entity.UpdatedAt = entity.CreatedAt;
      GetTable<T>().InsertOnSubmit(entity);
      _context.SubmitChanges();
      return entity.Id;
    }

    public int Update<T>(T entity) where T : class, ISeiyuuEntity<T> {
      if (!IsExists<T>(entity) || !entity.IsReadyEntity()) {
        return -1;
      }
      var target = GetEntity<T>(entity.Id);
      if (target is null) {
        return -1;
      }
      target.Replace(entity);
      entity.UpdatedAt = DateTime.Now;
      target.UpdatedAt = entity.UpdatedAt;
      _context.SubmitChanges();
      return target.Id;
    }

    public int Delete<T>(T entity) where T : class, ISeiyuuEntity<T> {
      if (typeof(T) == typeof(Actor) || typeof(T) == typeof(Anime) || typeof(T) == typeof(Company) || typeof(T) == typeof(Radio)) {
        //throw new NotImplementedException("The entity can be referred by another table, it may cause an error so you can't delete it now. We will implement the feature ASAP.");
        return -1;
      }

      var del = GetEntity<T>(entity);
      if (!IsExists(del)) {
        return -1;
      }
      GetTable<T>().DeleteOnSubmit(del);
      _context.SubmitChanges();
      return del.Id;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="picture_url">Picture Url(Accessible from anywhere)</param>
    /// <returns>Picture Internal Path(Accessible from local)</returns>
    public string SavePictureToBlob(string picture_url) {
      if (string.IsNullOrEmpty(picture_url)) {
        return null;
      } else {
        using (var client = new WebClient()) {
          var uri = new Uri(picture_url);
          var name = Path.GetFileName(uri.LocalPath);
          if (string.IsNullOrEmpty(name)) {
            return null;
          }
          var path = Path.Combine(BlobLocation, name);
          client.DownloadFile(picture_url, path);
          return path;
        }
      }
    }
  }
}
