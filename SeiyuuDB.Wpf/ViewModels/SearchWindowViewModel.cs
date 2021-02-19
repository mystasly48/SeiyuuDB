using SeiyuuDB.Core;
using SeiyuuDB.Core.Entities;
using SeiyuuDB.Wpf.Models;
using SeiyuuDB.Wpf.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SeiyuuDB.Wpf.ViewModels {
  public class SearchWindowViewModel : INotifyPropertyChanged {
    private ObservableCollection<TabItem> _tabItems;
    public ObservableCollection<TabItem> TabItems {
      get {
        return _tabItems;
      }
      set {
        _tabItems = value;
        OnPropertyChanged(nameof(TabItems));
      }
    }

    private int _selectedTabItemIndex;
    public int SelectedTabItemIndex {
      get {
        return _selectedTabItemIndex;
      }
      set {
        _selectedTabItemIndex = value;
        OnSelectedTabItemChanged();
        OnPropertyChanged(nameof(SelectedTabItemIndex));
      }
    }

    // 要検討
    public TabItem SelectedTabItem {
      get {
        return TabItems[SelectedTabItemIndex];
      }
    }

    private IEnumerable<ActorCardModel> _actorCardModels;
    public IEnumerable<ActorCardModel> ActorCardModels {
      get {
        return _actorCardModels;
      }
      set {
        _actorCardModels = value;
        OnPropertyChanged(nameof(ActorCardModels));
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

    // このプロパティ不要なので、コンバーターでどうにかできない？
    private string _searchKeywords;
    public string SearchKeywords {
      get {
        return _searchKeywords == null ? "" : _searchKeywords;
      }
      set {
        _searchKeywords = value;
        OnPropertyChanged(nameof(SearchKeywords));
      }
    }

    public string[] SearchKeywordsArray {
      get {
        return SearchKeywords.Split(' ', '　', ',', '、')
          .Where(x => !string.IsNullOrEmpty(x)).ToArray();
      }
    }

    private bool? _isFavoriteActor;
    public bool? IsFavoriteActor {
      get {
        return _isFavoriteActor;
      }
      set {
        _isFavoriteActor = value;
        OnPropertyChanged(nameof(IsFavoriteActor));
      }
    }

    private bool? _isCompletedActor;
    public bool? IsCompletedActor {
      get {
        return _isCompletedActor;
      }
      set {
        _isCompletedActor = value;
        OnPropertyChanged(nameof(IsCompletedActor));
      }
    }

    private bool _isLoading = true;
    public bool IsLoading {
      get {
        return _isLoading;
      }
      set {
        _isLoading = value;
        OnPropertyChanged(nameof(LoadingIndicatorVisibility));
        OnPropertyChanged(nameof(IsEnabledAddButton));
      }
    }

    public Visibility LoadingIndicatorVisibility {
      get {
        return IsLoading ? Visibility.Visible : Visibility.Collapsed;
      }
      set {
        IsLoading = value == Visibility.Visible;
        OnPropertyChanged(nameof(LoadingIndicatorVisibility));
        OnPropertyChanged(nameof(IsEnabledAddButton));
      }
    }

    public bool IsEnabledAddButton {
      get {
        return !IsLoading;
      }
    }

    public ICommand SearchCommand => new AnotherCommandImplementation(ExecuteSearch);
    public ICommand AdvancedSearchCommand => new AnotherCommandImplementation(ExecuteAdvancedSearch);
    public ICommand OpenActorCommand => new AnotherCommandImplementation(ExecuteOpenActor);
    public ICommand AddActorCommand => new AnotherCommandImplementation(ExecuteAddActor);

    private LocalSqlite _sqlite;

    public SearchWindowViewModel() {
      // 設定に移す
      _sqlite = new LocalSqlite(Information.SqliteFilePath, Information.BlobFolderPath);
      Initialize();
    }

    public void Initialize() {
      ExecuteSearch(null);
    }

    public void OnSelectedTabItemChanged() {
      Console.WriteLine("OnSelectedTabItemChanged");
    }

    private void ExecuteOpenActor(object obj) {
      Console.WriteLine("Execute Open Actor");
      Console.WriteLine(obj);
    }

    private void ExecuteAddActor(object obj) {
      Console.WriteLine("Execute Add Actor");
      Console.WriteLine(obj);
    }

    // Enter で反応するようになにかする
    private async void ExecuteSearch(object obj) {
      IsLoading = true;
      if (SearchKeywordsArray.Any()) {
        var result = await _sqlite.FindActorsByKeywords(SearchKeywordsArray);
        await UpdateActorsAsync(result);
      } else {
        await UpdateActorsAsync(_sqlite.FindActors());
      }
      IsLoading = false;
    }

    private async void ExecuteAdvancedSearch(object obj) {
      IsLoading = true;
      var favorite = IsFavoriteActor ?? false;
      var completed = IsCompletedActor ?? false;
      Actor[] result;
      if (SearchKeywordsArray.Any()) {
        result = await _sqlite.FindActorsByKeywords(SearchKeywordsArray);
      } else {
        result = _sqlite.FindActors();
      }
      result = result.Where(actor => actor.IsFavorite == favorite && actor.IsCompleted == completed).ToArray();
      await UpdateActorsAsync(result);
      IsLoading = true;
    }

    private async Task UpdateActorsAsync(Actor[] actors) {
      await Task.Run(() =>
          ActorCardModels = actors.Select(actor => new ActorCardModel(actor))
        );
    }

    public event PropertyChangedEventHandler PropertyChanged = null;
    protected void OnPropertyChanged(string message) {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(message));
    }
  }
}
