using SeiyuuDB.Core.Entities;
using System;
using System.Threading.Tasks;

namespace SeiyuuDB.Core {
  public interface ISeiyuuDB : IDisposable {
    int Insert<T>(T entity) where T : class, ISeiyuuEntity<T>;
    int Update<T>(T entity) where T : class, ISeiyuuEntity<T>;
    int Delete<T>(T entity) where T : class, ISeiyuuEntity<T>;
    bool IsExists<T>(T entity) where T : class, ISeiyuuEntity<T>;
    T GetEntity<T>(int id) where T : class, ISeiyuuEntity<T>;
    string SavePictureToBlob(string url);
    bool DeletePictureFromBlob(string url);

    Task<Actor[]> FindActorsByKeywords(string[] keywords);
    Actor FindActorById(int actorId);
    Actor FindActorByShortName(string shortName);
    Actor[] FindActors();
    string[] FindBirthdayActorNames();

    Anime FindAnimeById(int animeId);
    Anime FindAnimeByTitle(string title);
    Anime[] FindAnimes();

    AnimeCharacter[] FindAnimesCharactersByActorId(int actorId);
    AnimeCharacter[] FindAnimeCharactersByAnimeId(int animeId);

    Character FindCharacterById(int animeId);
    Character FindCharacterByNameAndActorId(string name, int actorId);
    Character[] FindCharacters();
    Character[] FindCharactersByActorId(int actorId);

    Company FindAgencyByName(string name);
    Company FindStationByName(string name);
    Company[] FindAgencies();
    Company[] FindStations();

    ExternalLink[] FindExternalLinksByActorId(int actorId);

    Game FindGameByTitle(string title);
    Game[] FindGames();

    GameCharacter[] FindGamesCharactersByActorId(int actorId);

    Note[] FindNotesByActorId(int actorId);

    OtherAppearance[] FindOtherAppearancesByActorId(int actorId);

    Radio FindRadioByTitle(string title);
    Radio[] FindRadios();

    RadioActor[] FindRadiosActorsByActorId(int actorId);
  }
}
