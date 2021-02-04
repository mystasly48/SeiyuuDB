using SeiyuuDB.Entities;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SeiyuuDB.Core {
  public class LocalSqlite : ISeiyuuDB, IDisposable {
    private SQLiteConnection _connection;
    private SQLiteCommand _command;
    private SeiyuuDataContext _context;

    public string DataSource { get; }
    public string BlobLocation { get; }

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

    private string getTableName<T>() where T : class, ISeiyuuEntity<T> {
      var attributes = typeof(T).GetCustomAttributes(typeof(TableAttribute), false);
      if (attributes.Count() > 0) {
        return (attributes.First() as TableAttribute).Name;
      } else {
        return typeof(T).Name;
      }
    }

    private int GetNextId<T>() where T : class, ISeiyuuEntity<T> {
      _command.CommandText = $"select id from {getTableName<T>()} order by rowid desc limit 1;";
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
      } else if (typeof(T) == typeof(AnimeCharacter)) {
        obj = _context.AnimesCharacters.FirstOrDefault(x => x.Id == id);
      } else if (typeof(T) == typeof(Game)) {
        obj = _context.Games.FirstOrDefault(x => x.Id == id);
      } else if (typeof(T) == typeof(GameCharacter)) {
        obj = _context.GamesCharacters.FirstOrDefault(x => x.Id == id);
      } else if (typeof(T) == typeof(Radio)) {
        obj = _context.Radios.FirstOrDefault(x => x.Id == id);
      } else if (typeof(T) == typeof(RadioActor)) {
        obj = _context.RadiosActors.FirstOrDefault(x => x.Id == id);
      } else if (typeof(T) == typeof(Company)) {
        obj = _context.Companies.FirstOrDefault(x => x.Id == id);
      } else if (typeof(T) == typeof(ExternalLink)) {
        obj = _context.ExternalLinks.FirstOrDefault(x => x.Id == id);
      } else if (typeof(T) == typeof(Note)) {
        obj  = _context.Notes.FirstOrDefault(x => x.Id == id);
      }
      return obj as T;
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
      // LastName を変更されたとき、
      // 条件あり：名前が存在していないのに登録できない
      // 条件なし：名前が存在しているのに登録できる
      // の問題が起きる
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

      GetTable<T>().DeleteOnSubmit(entity);
      _context.SubmitChanges();
      return entity.Id;
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

    public bool DeletePictureFromBlob(string pictureUrl) {
      if (File.Exists(pictureUrl)) {
        try {
          File.Delete(pictureUrl);
          return true;
        } catch (IOException ex) {
          Console.WriteLine(ex.Message);
          return false;
        }
      } else {
        return false;
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

    public async Task<Actor[]> FindActorsByKeywords(string[] keywords) {
      return await Task.Run(() => {
        IQueryable<Actor> result = null;
        foreach (var keyword in keywords) {
          string escaped = EscapedLikeQuery(keyword);
          //SqlMethods.Like(x.Title, escaped);
          //SqlMethods.Like(SqlFunctions.StringConvert((double)x.Height), escaped)

          var actorsByNameQuery = $"select id from actors" +
                                  $" where last_name || ifnull(first_name,'') like '{escaped}'" +
                                  $" or ifnull(last_name_kana,'') || ifnull(first_name_kana,'') like '{escaped}'" +
                                  $" or ifnull(last_name_romaji,'') || ifnull(first_name_romaji,'') like '{escaped}'";

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
                       || actor.SpouseName.Contains(keyword)
                       || actor.Agency.Name.Contains(keyword)
                       || actor.Agency.NameKana.Contains(keyword)
                       || actor.Agency.Alias.Contains(keyword)
                       select actor;

          var characters = from character in _context.Characters
                           where character.Name.Contains(keyword)
                           || character.NameKana.Contains(keyword)
                           || character.Alias.Contains(keyword)
                           select character.Actor;

          var animesCharacters = from film in _context.AnimesCharacters
                                   where film.Anime.Title.Contains(keyword)
                                   || film.Anime.TitleKana.Contains(keyword)
                                   || film.Anime.Alias.Contains(keyword)
                                   select film.Character.Actor;

          var gamesCharacters = from film in _context.GamesCharacters
                                  where film.Game.Title.Contains(keyword)
                                  || film.Game.TitleKana.Contains(keyword)
                                  || film.Game.Alias.Contains(keyword)
                                  select film.Character.Actor;

          var radiosActors = from film in _context.RadiosActors
                                   where film.Radio.Title.Contains(keyword)
                                   || film.Radio.TitleKana.Contains(keyword)
                                   || film.Radio.Alias.Contains(keyword)
                                   || film.Radio.Station.Name.Contains(keyword)
                                   || film.Radio.Station.NameKana.Contains(keyword)
                                   || film.Radio.Station.Alias.Contains(keyword)
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
              .Union(animesCharacters)
              .Union(gamesCharacters)
              .Union(radiosActors)
              .Union(links)
              .Union(notes);

          result = (result == null) ? actors : result.Union(actors);
        }
        return result.ToArray()
            .OrderBy(actor => actor.NameKana)
            .ThenBy(actor => actor.Name)
            .ToArray();
      });
    }

    public Actor FindActorById(int actorId) {
      var actors = from a in _context.Actors
                   where a.Id == actorId
                   select a;
      return actors.ToArray().FirstOrDefault();
    }

    public Actor FindActorByShortName(string shortName) {
      var query = $"select id from actors" +
                   $" where last_name || ifnull(first_name,'') == '{shortName}'" +
                   $" limit 1;";
      _command.CommandText = query;

      using (var reader = _command.ExecuteReader()) {
        while (reader.Read()) {
          var actorId = reader.GetInt32(0);
          return FindActorById(actorId);
        }
      }

      return null;
    }

    public Actor[] FindActors() {
      return _context.Actors.ToArray()
        .OrderBy(a => a.NameKana)
        .ThenBy(a => a.Name)
        .ToArray();
    }

    public string[] FindBirthdayActorNames() {
      return _context.Actors.ToArray()
        .Where(a => a.Birthdate.IsToday)
        .OrderByDescending(a => a.Birthdate.Age)
        .ThenBy(a => a.NameKana)
        .ThenBy(a => a.Name)
        .Select(a => a.ShortName)
        .ToArray();
    }

    public Anime FindAnimeByTitle(string title) {
      var animes = from a in _context.Animes
                   where a.Title == title
                   select a;
      return animes.ToArray().FirstOrDefault();
    }

    public Anime[] FindAnimes() {
      return _context.Animes.ToArray()
        .OrderBy(a => a.TitleKana)
        .ThenBy(a => a.Title)
        .ToArray();
    }

    public AnimeCharacter[] FindAnimesCharactersByActorId(int actorId) {
      var animesCharacters = from ac in _context.AnimesCharacters
                             where ac.Character.ActorId == actorId
                             select ac;
      return animesCharacters.ToArray()
        .OrderBy(ac => ac.Anime.ReleasedYear)
        .ThenBy(ac => ac.Anime.TitleKana)
        .ThenBy(ac => ac.Anime.Title)
        .ToArray();
    }

    public AnimeCharacter[] FindAnimeCharactersByAnimeId(int animeId) {
      var animesCharacters = from ac in _context.AnimesCharacters
                             where ac.AnimeId == animeId
                             select ac;
      return animesCharacters.ToArray()
        .OrderBy(ac => ac.Character.NameKana)
        .ThenBy(ac => ac.Character.Name)
        .ToArray();
    }

    public Character FindCharacterByNameAndActorId(string name, int actorId) {
      var characters = from c in _context.Characters
                       where c.Name == name
                       && c.ActorId == actorId
                       select c;
      return characters.ToArray().FirstOrDefault();
    }

    public Character[] FindCharacters() {
      return _context.Characters.ToArray()
        .OrderBy(c => c.NameKana)
        .ThenBy(c => c.Name)
        .ToArray();
    }

    public Character[] FindCharactersByActorId(int actorId) {
      var characters = from c in _context.Characters
                       where c.ActorId == actorId
                       select c;
      return characters.ToArray()
        .OrderBy(c => c.NameKana)
        .ThenBy(c => c.Name)
        .ToArray();
    }

    public Company FindAgencyByName(string name) {
      var agencies = from c in _context.Companies
                     where c.Name == name
                     && c.CompanyTypeId == (int)CompanyType.Agency
                     select c;
      return agencies.ToArray().FirstOrDefault();
    }
    
    public Company FindStationByName(string name) {
      var stations = from c in _context.Companies
                     where c.Name == name
                     && c.CompanyTypeId == (int)CompanyType.Station
                     select c;
      return stations.ToArray().FirstOrDefault();
    }

    public Company[] FindAgencies() {
      var agencies = from c in _context.Companies
                     where c.CompanyTypeId == (int)CompanyType.Agency
                     select c;
      return agencies.ToArray()
        .OrderBy(c => c.NameKana)
        .ThenBy(c => c.Name)
        .ToArray();
    }

    public Company[] FindStations() {
      var stations = from c in _context.Companies
                     where c.CompanyTypeId == (int)CompanyType.Station
                     select c;
      return stations.ToArray()
        .OrderBy(c => c.NameKana)
        .ThenBy(c => c.Name)
        .ToArray();
    }

    public ExternalLink[] FindExternalLinksByActorId(int actorId) {
      var externalLinks = from el in _context.ExternalLinks
                          where el.ActorId == actorId
                          select el;
      return externalLinks.ToArray()
        .OrderBy(el => el.CreatedAt)
        .ToArray();
    }

    public Game FindGameByTitle(string title) {
      var games = from g in _context.Games
                   where g.Title == title
                   select g;
      return games.ToArray().FirstOrDefault();
    }

    public Game[] FindGames() {
      return _context.Games.ToArray()
        .OrderBy(g => g.TitleKana)
        .ThenBy(g => g.Title)
        .ToArray();
    }

    public GameCharacter[] FindGamesCharactersByActorId(int actorId) {
      var gamesCharacters = from gc in _context.GamesCharacters
                            where gc.Character.ActorId == actorId
                            select gc;
      return gamesCharacters.ToArray()
        .OrderBy(gc => gc.Game.ReleasedYear)
        .ThenBy(gc => gc.Game.TitleKana)
        .ThenBy(gc => gc.Game.Title)
        .ToArray();
    }

    public Note[] FindNotesByActorId(int actorId) {
      var notes = from n in _context.Notes
                  where n.ActorId == actorId
                  select n;
      return notes.ToArray()
        .OrderBy(n => n.CreatedAt)
        .ToArray();
    }

    public OtherAppearance[] FindOtherAppearancesByActorId(int actorId) {
      var otherAppearances = from oa in _context.OtherAppearances
                             where oa.ActorId == actorId
                             select oa;
      return otherAppearances.ToArray()
        .OrderBy(oa => oa.AppearedOn)
        .ThenBy(oa => oa.TitleKana)
        .ThenBy(oa => oa.Title)
        .ToArray();
    }

    public Radio FindRadioByTitle(string title) {
      var radios = from r in _context.Radios
                   where r.Title == title
                   select r;
      return radios.ToArray().FirstOrDefault();
    }

    public Radio[] FindRadios() {
      return _context.Radios.ToArray()
        .OrderBy(r => r.TitleKana)
        .ThenBy(r => r.Title)
        .ToArray();
    }

    public RadioActor[] FindRadiosActorsByActorId(int actorId) {
      var radiosActors = from ra in _context.RadiosActors
                         where ra.ActorId == actorId
                         select ra;
      return radiosActors.ToArray()
        .OrderBy(ra => ra.Radio.StartedOn)
        .ThenBy(ra => ra.Radio.EndedOn)
        .ToArray();
    }

    private string EscapedLikeQuery(string originalQuery) {
      string escaped = originalQuery
        .Replace("/", "//")
        .Replace("_", "/_")
        .Replace("%", "/%")
        .Replace("[", "/[")
        .Replace("'", "''");
      return string.Format("%{0}%", escaped);
    }
  }
}
