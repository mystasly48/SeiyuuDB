using SeiyuuDB.Core.Entities;
using SeiyuuDB.Core.Helpers;
using SeiyuuDB.Wpf.Models;
using SeiyuuDB.Wpf.Utils;
using SeiyuuDB.Wpf.Views;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SeiyuuDB.Wpf.ViewModels {
  public class SearchTabViewModel : Observable {
    private IEnumerable<ActorCardModel> _actorCardModels;
    public IEnumerable<ActorCardModel> ActorCardModels {
      get => _actorCardModels;
      set {
        SetProperty(ref _actorCardModels, value);
        OnPropertyChanged(nameof(CurrentDisplayActorsCount));
        OnPropertyChanged(nameof(NotFoundTextVisibility));
      }
    }

    public string CurrentDisplayActorsCount {
      get {
        if (ActorCardModels == null || ActorCardModels.Count() == 0) {
          return "声優を検索";
        } else {
          return ActorCardModels.Count() + "人の声優を表示中";
        }
      }
    }

    public Visibility NotFoundTextVisibility {
      get {
        if (ActorCardModels != null && ActorCardModels.Count() == 0) {
          return Visibility.Visible;
        } else {
          return Visibility.Collapsed;
        }
      }
    }

    // TODO このプロパティ不要なので、コンバーターでどうにかできない？
    private string _searchKeywords;
    public string SearchKeywords {
      get => _searchKeywords ?? "";
      set => SetProperty(ref _searchKeywords, value);
    }

    public string[] SearchKeywordsArray =>
      SearchKeywords.Split(' ', '　', ',', '、')
          .Where(x => !string.IsNullOrEmpty(x)).ToArray();

    private bool? _isFavoriteActor;
    public bool? IsFavoriteActor {
      get => _isFavoriteActor;
      set => SetProperty(ref _isFavoriteActor, value);
    }

    private bool? _isCompletedActor;
    public bool? IsCompletedActor {
      get => _isCompletedActor;
      set => SetProperty(ref _isCompletedActor, value);
    }

    private bool _isLoading = true;
    public bool IsLoading {
      get => _isLoading;
      set {
        SetProperty(ref _isLoading, value);
        OnPropertyChanged(nameof(LoadingIndicatorVisibility));
        OnPropertyChanged(nameof(IsEnabledAddButton));
      }
    }

    public Visibility LoadingIndicatorVisibility {
      get => IsLoading ? Visibility.Visible : Visibility.Collapsed;
      set {
        IsLoading = value == Visibility.Visible;
        OnPropertyChanged(nameof(LoadingIndicatorVisibility));
        OnPropertyChanged(nameof(IsEnabledAddButton));
      }
    }

    public bool IsEnabledAddButton => !IsLoading;

    public ICommand SearchCommand => new AnotherCommandImplementation(ExecuteSearch);
    public ICommand AdvancedSearchCommand => new AnotherCommandImplementation(ExecuteAdvancedSearch);
    public ICommand OpenActorCommand => new AnotherCommandImplementation(ExecuteOpenActor);
    public ICommand AddActorCommand => new AnotherCommandImplementation(ExecuteAddActor);

    public SearchTabViewModel() {
      ExecuteSearch(null);
    }

    private void ExecuteOpenActor(object obj) {
      var actor = obj as Actor;
      if (obj is ActorCardModel model) {
        actor = DbManager.Connection.FindActorById(model.ActorId);
      }
      if (actor is null)
        return;

      TabManager.OpenNewTab(actor);
    }

    private void ExecuteAddActor(object obj) {
      // TODO 実装
      var actor = DbManager.Connection.FindActorById(1);
      var viewModel = new AddActorViewModel(actor);
      var form = new AddActorWindow(viewModel);
      form.ShowDialog();

      //if (form.Success) {
      //  await ExecuteSearch(null);
      //  ExecuteOpenActor(form.Actor);
      //}
    }

    private async void ExecuteSearch(object obj) {
      IsLoading = true;
      // TODO キーワードが変わるたびに自動で検索するようにしたいが、DBアクセスの負担が高い
      if (SearchKeywordsArray.Any()) {
        var result = await DbManager.Connection.FindActorsByKeywords(SearchKeywordsArray);
        await UpdateActorsAsync(result);
      } else {
        await UpdateActorsAsync(DbManager.Connection.FindActors());
      }
      IsLoading = false;
    }

    private async void ExecuteAdvancedSearch(object obj) {
      IsLoading = true;
      var favorite = IsFavoriteActor ?? false;
      var completed = IsCompletedActor ?? false;
      Actor[] result;
      if (SearchKeywordsArray.Any()) {
        result = await DbManager.Connection.FindActorsByKeywords(SearchKeywordsArray);
      } else {
        result = DbManager.Connection.FindActors();
      }
      result = result.Where(actor => actor.IsFavorite == favorite && actor.IsCompleted == completed).ToArray();
      await UpdateActorsAsync(result);
      IsLoading = false;
    }

    private async Task UpdateActorsAsync(Actor[] actors) {
      // TODO actors が変わらなければ変更を加えないようにしたい
      await Task.Run(() =>
          ActorCardModels = actors.Select(actor => new ActorCardModel(actor))
        );
    }
  }
}
