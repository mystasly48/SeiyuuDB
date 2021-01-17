using SeiyuuDB.Entities;
using System;
using System.Collections.Generic;
using System.Data.Linq;
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
      if (!IsExists(entity) || !entity.IsReadyEntity()) {
        return -1;
      }
      entity.UpdatedAt = DateTime.Now;
      _context.SubmitChanges();
      return entity.Id;
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

    public string SavePictureToBlob(string pictureUrl) {
      if (string.IsNullOrEmpty(pictureUrl)) {
        return null;
      } else {
        using (var client = new WebClient()) {
          var uri = new Uri(pictureUrl);
          var ext = Path.GetExtension(uri.LocalPath);
          if (string.IsNullOrEmpty(ext)) {
            return null;
          }
          ext = ".jpg";
          var name = getRandomName() + ext;
          var savedPath = Path.Combine(BlobLocation, name);
          while (File.Exists(savedPath)) {
            name = getRandomName() + ext;
            savedPath = Path.Combine(BlobLocation, name);
          }
          client.DownloadFile(pictureUrl, savedPath);

          Bitmap bmp = new Bitmap(savedPath);
          int resizeHeight = 400;
          int resizeWidth = (int)(bmp.Width * (resizeHeight / (double)bmp.Height));

          Bitmap resizedBmp = new Bitmap(resizeWidth, resizeHeight);
          Graphics g = Graphics.FromImage(resizedBmp);
          g.InterpolationMode = InterpolationMode.HighQualityBicubic;
          g.DrawImage(bmp, 0, 0, resizeWidth, resizeHeight);
          g.Dispose();
          bmp.Dispose();

          resizedBmp.Save(savedPath, ImageFormat.Jpeg);

          return savedPath;
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
          //SqlMethods.Like(x.Title, escaped);
          //SqlMethods.Like(SqlFunctions.StringConvert((double)x.Height), escaped)

          var actorsByNameQuery = $"select id from Actor" +
                                  $" where last_name || first_name like '{escaped}'" +
                                  $" or last_name_kana || first_name_kana like '{escaped}'" +
                                  $" or last_name_romaji || first_name_romaji like '{escaped}'";

          _command.CommandText = actorsByNameQuery;
          var actorIdsByName = new List<int>();
          using (var reader = _command.ExecuteReader()) {
            while (reader.Read()) {
              actorIdsByName.Add(reader.GetInt32(0));
            }
          }
          var actorsByName = from actor in _context.Actors
                             where actorIdsByName.Contains(actor.Id)
                             select actor;

          var actors = from actor in _context.Actors
                       where actor.Nickname.Contains(keyword)
                       // Gender
                       // Birthdate
                       // BloodType
                       || actor.Hometown.Contains(keyword)
                       // Height
                       // Debut
                       || actor.Spouse.Contains(keyword)
                       || actor.Agency.Name.Contains(keyword)
                       || actor.Agency.NameKana.Contains(keyword)
                       select actor;

          var characters = from character in _context.Characters
                           where character.Name.Contains(keyword)
                           || character.NameKana.Contains(keyword)
                           select character.Actor;

          var animeFilmographies = from film in _context.AnimeFilmographies
                                   where film.Anime.Title.Contains(keyword)
                                   || film.Anime.TitleKana.Contains(keyword)
                                   select film.Character.Actor;

          var gameFilmographies = from film in _context.GameFilmographies
                                  where film.Game.Title.Contains(keyword)
                                  || film.Game.TitleKana.Contains(keyword)
                                  select film.Character.Actor;

          var radioFilmographies = from film in _context.RadioFilmographies
                                   where film.Radio.Title.Contains(keyword)
                                   || film.Radio.TitleKana.Contains(keyword)
                                   || film.Radio.Station.Name.Contains(keyword)
                                   || film.Radio.Station.NameKana.Contains(keyword)
                                   select film.Actor;

          var links = from link in _context.ExternalLinks
                      where link.Title.Contains(keyword)
                      select link.Actor;

          var notes = from note in _context.Notes
                      where note.Title.Contains(keyword)
                      || note.Content.Contains(keyword)
                      select note.Actor;

          actors = actors
              .Union(actorsByName)
              .Union(characters)
              .Union(animeFilmographies)
              .Union(gameFilmographies)
              .Union(radioFilmographies)
              .Union(links)
              .Union(notes);

          result = (result == null) ? actors : result.Union(actors);
        }
        return result.ToArray();
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
