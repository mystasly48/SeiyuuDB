using MaterialDesignThemes.Wpf;
using SeiyuuDB.Wpf.ViewModels;
using SeiyuuDB.Wpf.Controls;
using System.ComponentModel;
using System.Windows;

namespace SeiyuuDB.Wpf.Views {
  /// <summary>
  /// AddActorWindow.xaml の相互作用ロジック
  /// </summary>
  public partial class AddActorWindow : Window {
    private bool IsAlreadyClosing = false;

    public AddActorWindow(AddActorViewModel viewModel) {
      InitializeComponent();
      this.DataContext = viewModel;
    }

    private async void AddActorWindow_Closing(object sender, CancelEventArgs e) {
      System.Console.WriteLine("IsAlreadyClosing: " + IsAlreadyClosing);
      if (IsAlreadyClosing) {
        e.Cancel = true;
        return;
      }
      if (this.DataContext is AddActorViewModel viewModel) {
        System.Console.WriteLine("IsChanged: " + viewModel.IsChanged);
        if (viewModel.IsChanged) {
          // Cancel to close is default
          e.Cancel = true;
          IsAlreadyClosing = true;
          var dialog = new YesNoCancelDialog("内容が変更されています。保存しますか？");
          var result = await DialogHost.Show(dialog, "AddActorWindow");
          if (result is MessageBoxResult resultType) {
            if (resultType == MessageBoxResult.Yes) {
              // Yes -> Apply and Close
              // TODO: Apply
              Visibility = Visibility.Collapsed;
              this.DataContext = null;
              Close();
            } else if (resultType == MessageBoxResult.No) {
              // No -> Dispose and Close
              Visibility = Visibility.Collapsed;
              this.DataContext = null;
              Close();
            }
          }
          IsAlreadyClosing = false;
        }
      }
    }
  }
}
