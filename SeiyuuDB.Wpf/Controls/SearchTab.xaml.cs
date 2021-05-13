using SeiyuuDB.Wpf.ViewModels;
using System.Windows.Controls;

namespace SeiyuuDB.Wpf.Controls {
  /// <summary>
  /// Interaction logic for SearchTab.xaml
  /// </summary>
  public partial class SearchTab : UserControl {
    public SearchTab(SearchTabViewModel viewModel) {
      InitializeComponent();
      this.DataContext = viewModel;
    }
  }
}
