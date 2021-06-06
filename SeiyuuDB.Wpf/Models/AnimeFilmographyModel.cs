using SeiyuuDB.Core.Entities;
using SeiyuuDB.Core.Helpers;
using SeiyuuDB.Wpf.Utils;
using System.Windows;
using System.Windows.Input;

namespace SeiyuuDB.Wpf.Models {
  public class AnimeFilmographyModel : Observable {
    private AnimeCharacter _animeCharacter;
    public AnimeCharacter AnimeCharacter {
      get => _animeCharacter;
      set {
        SetProperty(ref _animeCharacter, value);
        OnPropertyChanged(nameof(Title));
        OnPropertyChanged(nameof(ReleasedYear));
        OnPropertyChanged(nameof(CharacterName));
        OnPropertyChanged(nameof(IsMainRole));
        OnPropertyChanged(nameof(CharacterFontWeight));
      }
    }

    public string Title => AnimeCharacter.Anime.Title;
    public string ReleasedYear => AnimeCharacter.Anime.ReleasedYear + "年";
    public string CharacterName => AnimeCharacter.Character.Name;
    public bool IsMainRole => AnimeCharacter.Character.IsMainRole;
    public FontWeight CharacterFontWeight => IsMainRole ? FontWeights.UltraBold : FontWeights.Normal;

    public ICommand OpenAnimeCommand => new AnotherCommandImplementation(ExecuteOpenAnime);
    public ICommand OpenCharacterCommand => new AnotherCommandImplementation(ExecuteOpenCharacter);

    // TODO アニメウィンドウがない
    private void ExecuteOpenAnime(object obj) {
      var anime = DbManager.Connection.FindAnimeById(AnimeCharacter.Anime.Id);
    }

    // TODO キャラクタウィンドウがない
    private void ExecuteOpenCharacter(object obj) {
      var character = DbManager.Connection.FindCharacterById(AnimeCharacter.Character.Id);
    }

    public AnimeFilmographyModel(AnimeCharacter item) {
      AnimeCharacter = item;
    }
  }
}
