using SeiyuuDB.Wpf.ViewModels;
using System.Windows;

namespace SeiyuuDB.Wpf.Views {
  /// <summary>
  /// AddActorWindow.xaml の相互作用ロジック
  /// </summary>
  public partial class AddActorWindow : Window {
    public AddActorWindow(AddActorViewModel viewModel) {
      InitializeComponent();
      this.DataContext = viewModel;
    }
  }
}
