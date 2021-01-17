using SeiyuuDB.Entities;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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
    public Character[] Characters => _context.Characters.ToArray();
    public Company[] Companies => _context.Companies.ToArray();
    public ExternalLink[] ExternalLinks => _context.ExternalLinks.ToArray();
    public Game[] Games => _context.Games.ToArray();
    public GameFilmography[] GameFilmographies => _context.GameFilmographies.ToArray();
    public Note[] Notes => _context.Notes.ToArray();
    public OtherFilmography[] OtherFilmographies => _context.OtherFilmographies.ToArray();
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
          var ext = Path.GetExtension(uri.LocalPath);
          if (string.IsNullOrEmpty(ext)) {
            return null;
          }
          var name = getRandomName() + ext;
          var path = Path.Combine(BlobLocation, name);
          while (File.Exists(path)) {
            name = getRandomName() + ext;
            path = Path.Combine(BlobLocation, name);
          }
          client.DownloadFile(picture_url, path);
          return path;
        }
      }
    }

    private string getRandomName() {
      var rand = new Random();
      const string BASE = "abcdefghijklmnopqrstuvwxyz0123456789";
      const int SIZE = 16;
      string res = string.Empty;
      for (int i = 0; i < SIZE; i++) {
        res += BASE[rand.Next(0, BASE.Length)];
      }
      return res;
    }

    public Actor FindActor(int actorId) {
      return _context.Actors.FirstOrDefault(x => x.Id == actorId);
    }

    public async Task<Actor[]> FindActorsAsync(string[] keywords) {
      return await Task.Run(() => {
        IQueryable<Actor> result = null;
        foreach (var keyword in keywords) {
          string escaped = EscapedLikeQuery(keyword);

          // TODO もっと
          var actor = _context.Actors
            .Where(x => SqlMethods.Like(x.FirstName, escaped)
            || SqlMethods.Like(x.LastName, escaped)
            || SqlMethods.Like(x.FirstNameKana, escaped)
            || SqlMethods.Like(x.LastNameKana, escaped)
            || SqlMethods.Like(x.FirstNameRomaji, escaped)
            || SqlMethods.Like(x.LastNameRomaji, escaped)
            || SqlMethods.Like(x.Nickname, escaped)
            // Gender
            // Birthdate
            // BloodType
            //|| SqlMethods.Like(SqlFunctions.StringConvert((double)x.Height), escaped)
            || SqlMethods.Like(x.Hometown, escaped)
            //|| SqlMethods.Like(SqlFunctions.StringConvert((double)x.Debut), escaped)
            || SqlMethods.Like(x.Spouse, escaped)
            || SqlMethods.Like(x.Agency.Name, escaped)
            || SqlMethods.Like(x.Agency.NameKana, escaped));

          actor = actor.Union(_context.AnimeFilmographies
            .Where(x => SqlMethods.Like(x.Character.Name, escaped)
            || SqlMethods.Like(x.Character.NameKana, escaped)
            || SqlMethods.Like(x.Anime.Title, escaped)
            || SqlMethods.Like(x.Anime.TitleKana, escaped))
            .Select(x => x.Character.Actor));

          actor = actor.Union(_context.GameFilmographies
            .Where(x => SqlMethods.Like(x.Character.Name, escaped)
            || SqlMethods.Like(x.Character.NameKana, escaped)
            || SqlMethods.Like(x.Game.Title, escaped)
            || SqlMethods.Like(x.Game.TitleKana, escaped))
            .Select(x => x.Character.Actor));

          actor = actor.Union(_context.RadioFilmographies
            .Where(x => SqlMethods.Like(x.Radio.Title, escaped)
            || SqlMethods.Like(x.Radio.TitleKana, escaped)
            || SqlMethods.Like(x.Radio.Station.Name, escaped)
            || SqlMethods.Like(x.Radio.Station.NameKana, escaped))
            .Select(x => x.Actor));

          actor = actor.Union(_context.ExternalLinks
            .Where(x => SqlMethods.Like(x.Title, escaped))
            .Select(x => x.Actor));

          actor = actor.Union(_context.Notes
            .Where(x => SqlMethods.Like(x.Title, escaped)
            || SqlMethods.Like(x.Content, escaped))
            .Select(x => x.Actor));

          result = (result == null) ? actor : result.Union(actor);
        }
        return result.OrderBy(actor => actor.NameRomaji).ToArray();
      });
    }

    public ExternalLink[] FindExternalLinks(int actorId) {
      var externalLinks = from link in _context.ExternalLinks
                          where link.Actor.Id == actorId
                          select link;
      return externalLinks.ToArray();
    }
    
    public Note[] FindNotes(int actorId) {
      var notes = from note in _context.Notes
                  where note.Actor.Id == actorId
                  select note;
      return notes.ToArray();
    }

    public Character[] FindCharacters(int actorId) {
      var characters = from character in _context.Characters
                       where character.Actor.Id == actorId
                       select character;
      return characters.ToArray();
    }

    public RadioFilmography[] FindRadioFilmographies(int actorId) {
      var radioFilmographies = from film in _context.RadioFilmographies
                               where film.Actor.Id == actorId
                               select film;
      return radioFilmographies.ToArray();
    }

    public Anime FindAnime(string title) {
      return _context.Animes.FirstOrDefault(x => x.Title == title);
    }

    public Character FindCharacter(string name, int actorId) {
      return _context.Characters.FirstOrDefault(x => x.Name == name && x.Actor.Id == actorId);
    }

    private string EscapedLikeQuery(string originalQuery) {
      string escaped = originalQuery
        .Replace("/", "//")
        .Replace("_", "/_")
        .Replace("%", "/%")
        .Replace("[", "/[");
      return string.Format("%{0}%", escaped);
    }
  }
}
