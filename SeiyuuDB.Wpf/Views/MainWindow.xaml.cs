using SeiyuuDB.Wpf.ViewModels;
using System.Windows;

namespace SeiyuuDB.Wpf.Views {
  /// <summary>
  /// Interaction logic for SearchWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
      this.DataContext = new MainWindowViewModel();
    }
  }
}
