using SeiyuuDB.Core;
using SeiyuuDB.Core.Helpers;
using SeiyuuDB.Wpf.Controls;
using SeiyuuDB.Wpf.Utils;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace SeiyuuDB.Wpf.ViewModels {
  public class MainWindowViewModel : Observable {
    public ICommand ClosingCommand => new AnotherCommandImplementation(ExecuteClosingCommand);

    public MainWindowViewModel() {
      DbManager.Connection = new LocalSqlite(Information.SqliteFilePath, Information.BlobFolderPath);
      TabManager.TabItems = new ObservableCollection<TabItem>() {
        new TabItem() {
          Header = "検索",
          Content = new SearchTab(new SearchTabViewModel()),
          IsSelected = true
        }
      };
    }

    public void OnSelectedTabItemChanged() {
      Console.WriteLine("OnSelectedTabItemChanged");
    }

    private void ExecuteClosingCommand(object obj) {
      Console.WriteLine("ExecuteClosingCommand");
    }
  }
}
