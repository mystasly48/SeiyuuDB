using SeiyuuDB.Core.Entities;
using SeiyuuDB.Wpf.Models;
using SeiyuuDB.Wpf.Utils;
using System.Windows.Input;

namespace SeiyuuDB.Wpf.ViewModels {
  public class ActorTabViewModel : Observable {
    private ActorModel _actorModel;
    public ActorModel ActorModel {
      get {
        return _actorModel;
      }
      set {
        SetProperty(ref _actorModel, value);
      }
    }

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
      //var form = new AddActorWindow(ActorModel.Actor);
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
      ActorModel = new ActorModel() { Actor = actor };
    }
  }
}
