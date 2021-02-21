using SeiyuuDB.Wpf.Utils;
using System.Windows;

namespace SeiyuuDB.Wpf.Models {
  public class AnimeFilmographyModel : Observable {
    private string _title;
    public string Title {
      get => _title;
      set => SetProperty(ref _title, value);
    }

    private int _releasedYear;
    public int ReleasedYear {
      get => _releasedYear;
      set => SetProperty(ref _releasedYear, value);
    }

    private string _characterName;
    public string CharacterName {
      get => _characterName;
      set => SetProperty(ref _characterName, value);
    }

    private bool _isMainRole;
    public bool IsMainRole {
      get => _isMainRole;
      set {
        SetProperty(ref _isMainRole, value);
        OnPropertyChanged(nameof(CharacterFontWeight));
      }
    }

    public FontWeight CharacterFontWeight {
      get => IsMainRole ? FontWeights.UltraBold : FontWeights.Normal;
    }
  }
}
