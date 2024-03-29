﻿using Dragablz;
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
        var newTabItem = new TabItem() {
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
      var existsItem = TabItems.FirstOrDefault(x => x.Header.ToString() == actor.ShortName);
      return TabItems.IndexOf(existsItem);
    }

    public static void SwitchTab(int index) {
      if (TabItems.Count > index) {
        SelectedTabItemIndex = index;
      }
    }

    public static void CloseTab(int index) {
      if (index > 0) {
        TabItems.RemoveAt(index);
        if (index == TabItems.Count) {
          SwitchTab(index - 1);
        } else {
          SwitchTab(index);
        }
      }
    }

    public static void CloseCurrentTab() {
      CloseTab(SelectedTabItemIndex);
    }

    public static ICommand SwitchNumTabCommand => new AnotherCommandImplementation(ExecuteSwitchNumTab);
    public static ICommand CloseCurrentTabCommand => new AnotherCommandImplementation(ExecuteCloseCurrentTab);
    public static ICommand CloseTabByMiddleClickCommand => new AnotherCommandImplementation(ExecuteCloseTabByMiddleClick);

    private static void ExecuteSwitchNumTab(object obj) {
      if (obj is string indexStr) {
        if (int.TryParse(indexStr, out int index)) {
          SwitchTab(index);
        }
      }
    }

    private static void ExecuteCloseCurrentTab(object obj) {
      CloseCurrentTab();
    }

    private static void ExecuteCloseTabByMiddleClick(object obj) {
      var element = (TabablzControl)obj;
      var pos = Mouse.GetPosition(element);
      var headers = element.GetOrderedHeaders().ToArray();
      if (pos.Y > headers[0].ActualHeight ||
        headers.Length < 2) {
        return;
      }

      double width = 0;
      for (int i = 0; i < headers.Length; i++) {
        var header = headers[i];
        if (width <= pos.X && pos.X <= width + header.ActualWidth) {
          CloseTab(i);
          return;
        }
        width += header.ActualWidth;
      }
    }

    public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
    private static void NotifyStaticPropertyChanged([CallerMemberName] string propertyName = "") {
      StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
    }
  }
}
