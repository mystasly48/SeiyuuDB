using SeiyuuDB.Core.Entities;
using SeiyuuDB.Core.Helpers;
using SeiyuuDB.Wpf.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SeiyuuDB.Wpf.ViewModels {
  public class ActorTabViewModel : Observable {
    private Actor _actor;
    public Actor Actor {
      get {
        return _actor;
      }
      set {
        SetProperty(ref _actor, value);
        OnPropertyChanged(Name);
        OnPropertyChanged(NameRomaji);
        OnPropertyChanged(NameKana);
        OnPropertyChanged(Nickname);
        OnPropertyChanged(Gender);
        OnPropertyChanged(Birthdate);
        OnPropertyChanged(BloodType);
        OnPropertyChanged(Height);
        OnPropertyChanged(Hometown);
        OnPropertyChanged(DebutYear);
        OnPropertyChanged(SpouseName);
        OnPropertyChanged(AgencyName);
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
    public BitmapImage Picture => string.IsNullOrEmpty(Actor.PictureUrl) ? ImageHelper.NoImage : ImageHelper.UrlToBitmapImage(Actor.PictureUrl);

    private bool _isCompleted;
    public bool IsCompleted {
      get {
        return _isCompleted;
      }
      set {
        SetProperty(ref _isCompleted, value);
      }
    }

    private bool _isFavorite;
    public bool IsFavorite {
      get {
        return _isFavorite;
      }
      set {
        SetProperty(ref _isFavorite, value);
      }
    }

    public ICommand EditInformationCommand => new AnotherCommandImplementation(ExecuteEditInformation);
    public ICommand AddAnimeFilmographyCommand => new AnotherCommandImplementation(ExecuteAddAnimeFilmography);
    public ICommand AddGameFilmographyCommand => new AnotherCommandImplementation(ExecuteAddGameFilmography);
    public ICommand AddNoteCommand => new AnotherCommandImplementation(ExecuteAddNote);
    public ICommand AddExternalLinkCommand => new AnotherCommandImplementation(ExecuteAddExternalLink);
    public ICommand DeleteActorCommand => new AnotherCommandImplementation(ExecuteDeleteActor);

    private void ExecuteEditInformation(object obj) {
      //var form = new AddActorWindow(Actor);
      //form.Owner = Window.GetWindow(this);
      //form.ShowDialog();
      //if (form.Success) {
      //  Actor = form.Actor;
      //}
    }

    private void ExecuteAddAnimeFilmography(object obj) {

    }

    private void ExecuteAddGameFilmography(object obj) {

    }

    private void ExecuteAddNote(object obj) {

    }

    private void ExecuteAddExternalLink(object obj) {

    }

    private void ExecuteDeleteActor(object obj) {

    }

    public ActorTabViewModel(Actor actor) {
      Actor = actor;
    }
  }
}
