using SeiyuuDB.Wpf.ViewModels;
using System.Windows.Controls;

namespace SeiyuuDB.Wpf.Controls {
  /// <summary>
  /// YesNoCancelDialog.xaml の相互作用ロジック
  /// </summary>
  public partial class YesNoCancelDialog : UserControl {
    public YesNoCancelDialog(string message) {
      InitializeComponent();
      this.DataContext = new YesNoCancelDialogViewModel(message);
    }
  }
}
