using SeiyuuDB.Entities;
using System;
using System.Data.Linq;
using System.Data.SQLite;
using System.Linq;

namespace SeiyuuDB.Databases {
  public class LocalSqlite : ISeiyuuDB, IDisposable {
    private SQLiteConnection _connection;
    private SQLiteCommand _command;
    private DataContext _context;

    private Table<Actor> _actorTable;
    private Table<Anime> _animeTable;
    private Table<AnimeFilmography> _animeFilmographyTable;
    private Table<Game> _gameTable;
    private Table<GameFilmography> _gameFilmographyTable;
    private Table<Radio> _radioTable;
    private Table<RadioFilmography> _radioFilmographyTable;
    private Table<Company> _companyTable;
    private Table<ExternalLink> _externalLinkTable;
    private Table<Note> _noteTable;

    public string DataSource { get; }

    public LocalSqlite(string source) {
      this.DataSource = source;
      var connStr = new SQLiteConnectionStringBuilder() { DataSource = this.DataSource };
      _connection = new SQLiteConnection(connStr.ToString());
      _connection.Open();
      _command = new SQLiteCommand(_connection);
      _context = new DataContext(_connection);

      _actorTable = _context.GetTable<Actor>();
      _animeTable = _context.GetTable<Anime>();
      _animeFilmographyTable = _context.GetTable<AnimeFilmography>();
      _gameTable = _context.GetTable<Game>();
      _gameFilmographyTable = _context.GetTable<GameFilmography>();
      _radioTable = _context.GetTable<Radio>();
      _radioFilmographyTable = _context.GetTable<RadioFilmography>();
      _companyTable = _context.GetTable<Company>();
      _externalLinkTable = _context.GetTable<ExternalLink>();
      _noteTable = _context.GetTable<Note>();
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
      object obj = null;
      if (typeof(T) == typeof(Actor)) {
        obj = _actorTable;
      } else if (typeof(T) == typeof(Anime)) {
        obj = _animeTable;
      } else if (typeof(T) == typeof(AnimeFilmography)) {
        obj = _animeFilmographyTable;
      } else if (typeof(T) == typeof(Game)) {
        obj = _gameTable;
      } else if (typeof(T) == typeof(GameFilmography)) {
        obj = _gameFilmographyTable;
      } else if (typeof(T) == typeof(Radio)) {
        obj = _radioTable;
      } else if (typeof(T) == typeof(RadioFilmography)) {
        obj = _radioFilmographyTable;
      } else if (typeof(T) == typeof(Company)) {
        obj = _companyTable;
      } else if (typeof(T) == typeof(ExternalLink)) {
        obj = _externalLinkTable;
      } else if (typeof(T) == typeof(Note)) {
        obj = _noteTable;
      }
      return obj as Table<T>;
    }

    public T[] GetTableArray<T>() where T : class, ISeiyuuEntity<T> {
      object[] obj = null;
      if (typeof(T) == typeof(Actor)) {
        obj = _actorTable
          .Join(_companyTable, x => x.AgencyId, y => y.Id, (x, y) => new { x, y })
          .ToArray()
          .Select(x => new Actor(x.x, x.y))
          .ToArray();
      } else if (typeof(T) == typeof(Anime)) {
        obj = _animeTable.ToArray();
      } else if (typeof(T) == typeof(AnimeFilmography)) {
        obj = _animeFilmographyTable
          .Join(_actorTable, a => a.ActorId, b => b.Id, (a, b) => new { a, b })
          .Join(_companyTable, a => a.b.AgencyId, c => c.Id, (a, c) => new { a.a, a.b, c })
          .Join(_animeTable, a => a.a.AnimeId, d => d.Id, (a, d) => new { a.a, a.b, a.c, d })
          .ToArray()
          .Select(x => new AnimeFilmography(x.a, new Actor(x.b, x.c), x.d))
          .ToArray();
      } else if (typeof(T) == typeof(Game)) {
        obj = _gameTable.ToArray();
      } else if (typeof(T) == typeof(GameFilmography)) {
        obj = _gameFilmographyTable
          .Join(_actorTable, a => a.ActorId, b => b.Id, (a, b) => new { a, b })
          .Join(_companyTable, a => a.b.AgencyId, c => c.Id, (a, c) => new { a.a, a.b, c })
          .Join(_gameTable, a => a.a.GameId, d => d.Id, (a, d) => new { a.a, a.b, a.c, d })
          .ToArray()
          .Select(x => new GameFilmography(x.a, new Actor(x.b, x.c), x.d))
          .ToArray();
      } else if (typeof(T) == typeof(Radio)) {
        obj = _radioTable
          .Join(_companyTable, x => x.StationId, y => y.Id, (x, y) => new { x, y })
          .ToArray()
          .Select(x => new Radio(x.x, x.y))
          .ToArray();
      } else if (typeof(T) == typeof(RadioFilmography)) {
        obj = _radioFilmographyTable
          .Join(_actorTable, a => a.ActorId, b => b.Id, (a, b) => new { a, b })
          .Join(_radioTable, a => a.a.RadioId, c => c.Id, (a, c) => new { a.a, a.b, c })
          .Join(_companyTable, a => a.b.AgencyId, d => d.Id, (a, d) => new { a.a, a.b, a.c, d })
          .Join(_companyTable, a => a.c.StationId, e => e.Id, (a, e) => new { a.a, a.b, a.c, a.d, e })
          .ToArray()
          .Select(x => new RadioFilmography(x.a, new Actor(x.b, x.d), new Radio(x.c, x.e)))
          .ToArray();
      } else if (typeof(T) == typeof(Company)) {
        obj = _companyTable.ToArray();
      } else if (typeof(T) == typeof(ExternalLink)) {
        obj = _externalLinkTable
          .Join(_actorTable, a => a.ActorId, b => b.Id, (a, b) => new { a, b })
          .Join(_companyTable, a => a.b.AgencyId, c => c.Id, (a, c) => new { a.a, a.b, c })
          .ToArray()
          .Select(x => new ExternalLink(x.a, new Actor(x.b, x.c)))
          .ToArray();
      } else if (typeof(T) == typeof(Note)) {
        obj = _noteTable
          .Join(_actorTable, a => a.ActorId, b => b.Id, (a, b) => new { a, b })
          .Join(_companyTable, a => a.b.AgencyId, c => c.Id, (a, c) => new { a.a, a.b, c })
          .ToArray()
          .Select(x => new Note(x.a, new Actor(x.b, x.c)))
          .ToArray();
      }
      return obj as T[];
    }

    public T GetEntity<T>(int id) where T : class, ISeiyuuEntity<T> {
      return GetTableArray<T>().FirstOrDefault(x => x.Id == id);
    }

    public T GetEntity<T>(T entity) where T : class, ISeiyuuEntity<T> {
      return GetTableArray<T>().FirstOrDefault(x => x.Equals(entity));
    }

    public bool IsExists<T>(T entity) where T : class, ISeiyuuEntity<T> {
      return GetTableArray<T>().Any(x => x.Equals(entity));
    }

    public int Insert<T>(T entity) where T : class, ISeiyuuEntity<T> {
      if (IsExists(entity) || !entity.IsReadyEntityWithoutId()) {
        return -1;
      }
      if (!entity.IsReadyEntity()) {
        entity.Id = GetNextId<T>();
      }
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
        throw new NotImplementedException("指定されたエンティティは削除すると他のテーブルにも影響があるので現時点では実装していません");
      }

      var del = GetEntity<T>(entity);
      if (!IsExists(del)) {
        return -1;
      }
      GetTable<T>().DeleteOnSubmit(del);
      _context.SubmitChanges();
      return del.Id;
    }
  }
}
