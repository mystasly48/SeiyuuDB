using SeiyuuDB.Core.Entities;
using SeiyuuDB.Wpf.Controls;
using SeiyuuDB.Wpf.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;

namespace SeiyuuDB.Wpf.Utils {
  public static class TabManager {
    private static ObservableCollection<TabItem> _tabItems;
    public static ObservableCollection<TabItem> TabItems {
      get {
        return _tabItems;
      }
      set {
        _tabItems = value;
        NotifyStaticPropertyChanged();
      }
    }

    private static int _selectedTabItemIndex;
    public static int SelectedTabItemIndex {
      get {
        return _selectedTabItemIndex;
      }
      set {
        _selectedTabItemIndex = value;
        NotifyStaticPropertyChanged();
      }
    }

    public static void OpenNewTab(Actor actor) {
      int existsActorTabIndex = GetTabIndex(actor);
      if (existsActorTabIndex == -1) {
        TabItem newTabItem = new TabItem() {
          Header = actor.ShortName,
          Content = new ActorTab(new ActorTabViewModel(actor))
        };
        TabItems.Add(newTabItem);
        SelectedTabItemIndex = TabItems.Count() - 1;
      } else {
        SelectedTabItemIndex = existsActorTabIndex;
      }
    }

    private static int GetTabIndex(Actor actor) {
      TabItem existsItem = TabItems.FirstOrDefault(x => x.Header.ToString() == actor.ShortName);
      return TabItems.IndexOf(existsItem);
    }

    public static void SwitchTab(int index) {
      if (TabItems.Count > index) {
        SelectedTabItemIndex = index;
      }
    }

    public static void CloseCurrentTab() {
      if (SelectedTabItemIndex > 0) {
        var index = SelectedTabItemIndex;
        SelectedTabItemIndex = index - 1;
        TabItems.RemoveAt(index);
      }
    }

    public static ICommand SwitchNumTabCommand => new AnotherCommandImplementation(ExecuteSwitchNumTab);
    public static ICommand CloseCurrentTabCommand => new AnotherCommandImplementation(ExecuteCloseCurrentTab);

    private static void ExecuteSwitchNumTab(object obj) {
      if (obj is string indexStr) {
        int index;
        if (int.TryParse(indexStr, out index)) {
          SwitchTab(index);
        }
      }
    }

    private static void ExecuteCloseCurrentTab(object obj) {
      CloseCurrentTab();
    }

    public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
    private static void NotifyStaticPropertyChanged([CallerMemberName] string propertyName = "") {
      StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
    }
  }
}
