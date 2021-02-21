using SeiyuuDB.Wpf.ViewModels;
using System.Windows.Controls;

namespace SeiyuuDB.Wpf.Controls {
  /// <summary>
  /// Interaction logic for ActorTabItem.xaml
  /// </summary>
  public partial class ActorTab : UserControl {
    public ActorTab(ActorTabViewModel viewModel) {
      InitializeComponent();
      this.DataContext = viewModel;
    }
  }
}
