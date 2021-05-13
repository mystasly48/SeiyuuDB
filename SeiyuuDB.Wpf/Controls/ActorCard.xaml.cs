using SeiyuuDB.Wpf.Models;
using System.Windows;
using System.Windows.Controls;

namespace SeiyuuDB.Wpf.Controls {
  /// <summary>
  /// Interaction logic for ActorCard.xaml
  /// </summary>
  public partial class ActorCard : UserControl {
    public ActorCardModel ActorCardModel {
      get { return (ActorCardModel)GetValue(ActorCardModelProperty); }
      set { SetValue(ActorCardModelProperty, value); }
    }
    public static readonly DependencyProperty ActorCardModelProperty =
        DependencyProperty.Register("ActorCardModel", typeof(ActorCardModel), typeof(ActorCard), new PropertyMetadata(null));

    public ActorCard() {
      InitializeComponent();
    }
  }
}
