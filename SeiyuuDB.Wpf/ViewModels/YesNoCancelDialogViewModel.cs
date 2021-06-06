using SeiyuuDB.Wpf.Utils;

namespace SeiyuuDB.Wpf.ViewModels {
  public class YesNoCancelDialogViewModel : Observable {
    private string _message;
    public string Message {
      get => _message;
      set => SetProperty(ref _message, value);
    }

    public YesNoCancelDialogViewModel(string message) {
      this.Message = message;
    }
  }
}
