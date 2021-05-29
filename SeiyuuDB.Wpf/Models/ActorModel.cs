using SeiyuuDB.Core.Entities;
using SeiyuuDB.Core.Helpers;
using SeiyuuDB.Wpf.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace SeiyuuDB.Wpf.Models {
  public class ActorModel : Observable {
    private Actor _actor;
    public Actor Actor {
      get {
        return _actor;
      }
      set {
        SetProperty(ref _actor, value);
        OnPropertyChanged(nameof(Name));
        OnPropertyChanged(nameof(NameRomaji));
        OnPropertyChanged(nameof(NameKana));
        OnPropertyChanged(nameof(Nickname));
        OnPropertyChanged(nameof(Gender));
        OnPropertyChanged(nameof(Birthdate));
        OnPropertyChanged(nameof(BloodType));
        OnPropertyChanged(nameof(Height));
        OnPropertyChanged(nameof(Hometown));
        OnPropertyChanged(nameof(DebutYear));
        OnPropertyChanged(nameof(SpouseName));
        OnPropertyChanged(nameof(AgencyName));
        OnPropertyChanged(nameof(Picture));
        OnPropertyChanged(nameof(AnimeFilmographyModels));
      }
    }

    public string Name => Actor.Name;
    public string NameRomaji => Actor.NameRomaji;
    public string NameKana => Actor.NameKana;
    public string Nickname => Actor.Nickname;
    public string Gender => EnumHelper.DisplayName(Actor.Gender);
    public string Birthdate {
      get {
        string res = Actor.Birthdate.ToString();
        if (Actor.Birthdate.Age.HasValue) {
          res += "（" + Actor.Birthdate.Age.Value + "歳）";
        }
        return res;
      }
    }
    public string BloodType => EnumHelper.DisplayName(Actor.BloodType);
    public string Height => Actor.Height.HasValue ? Actor.Height.Value.ToString() + "cm" : "";
    public string Hometown => Actor.Hometown;
    public string DebutYear => Actor.DebutYear.HasValue ? Actor.DebutYear.Value.ToString() + "年" : "";
    public string SpouseName => Actor.SpouseName;
    public string AgencyName => Actor.Agency?.Name; // ボタンにする
    public BitmapImage Picture {
      get {
        return string.IsNullOrEmpty(Actor.PictureUrl)
          ? ImageHelper.NoImage
          : ImageHelper.UrlToBitmapImage(Actor.PictureUrl);
      }
    }

    public IEnumerable<AnimeFilmographyModel> AnimeFilmographyModels {
      get => DbManager.Connection.FindAnimesCharactersByActorId(Actor.Id)
          .Select(item => new AnimeFilmographyModel(item));
    }

    public IEnumerable<RadioFilmographyModel> RadioFilmographyModels {
      get => DbManager.Connection.FindRadiosActorsByActorId(Actor.Id)
          .Select(item => new RadioFilmographyModel(item));
    }
  }
}
