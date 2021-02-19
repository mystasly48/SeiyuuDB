using SeiyuuDB.Core.Entities;
using System.Data;
using System.Data.Linq;

namespace SeiyuuDB.Core {
  public class SeiyuuDataContext : DataContext {
    public Table<Actor> Actors;
    public Table<Anime> Animes;
    public Table<AnimeCharacter> AnimesCharacters;
    public Table<Character> Characters;
    public Table<Company> Companies;
    public Table<ExternalLink> ExternalLinks;
    public Table<Game> Games;
    public Table<GameCharacter> GamesCharacters;
    public Table<Note> Notes;
    public Table<OtherAppearance> OtherAppearances;
    public Table<Radio> Radios;
    public Table<RadioActor> RadiosActors;

    public SeiyuuDataContext(IDbConnection connection) : base(connection) { }
  }
}
