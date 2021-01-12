using SeiyuuDB.Entities;
using System.Data;
using System.Data.Linq;

namespace SeiyuuDB.Databases {
  public class SeiyuuDataContext : DataContext {
    public Table<Actor> Actors;
    public Table<Anime> Animes;
    public Table<AnimeFilmography> AnimeFilmographies;
    public Table<Company> Companies;
    public Table<ExternalLink> ExternalLinks;
    public Table<Game> Games;
    public Table<GameFilmography> GameFilmographies;
    public Table<Note> Notes;
    public Table<OtherFilmography> OtherFilmographies;
    public Table<Radio> Radios;
    public Table<RadioFilmography> RadioFilmographies;

    public SeiyuuDataContext(IDbConnection connection) : base(connection) { }
  }
}
