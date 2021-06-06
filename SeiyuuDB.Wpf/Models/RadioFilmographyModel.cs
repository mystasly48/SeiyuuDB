using SeiyuuDB.Core.Entities;
using SeiyuuDB.Core.Helpers;
using SeiyuuDB.Wpf.Utils;
using System.Windows.Input;

namespace SeiyuuDB.Wpf.Models {
  public class RadioFilmographyModel : Observable {
    private RadioActor _radioActor;
    public RadioActor RadioActor {
      get => _radioActor;
      set {
        SetProperty(ref _radioActor, value);
        OnPropertyChanged(nameof(Title));
        OnPropertyChanged(nameof(StationName));
        OnPropertyChanged(nameof(StreamingTerm));
      }
    }

    public string Title => RadioActor.Radio.Title;
    public string StationName => RadioActor.Radio.Station.Name;
    public string StreamingTerm => string.Format("{0} - {1}",
          RadioActor.Radio.StartedOn.HasValue ? RadioActor.Radio.StartedOn.Value.Year + "年" : "",
          RadioActor.Radio.EndedOn.HasValue ? RadioActor.Radio.EndedOn.Value.Year + "年" : "");

    public ICommand OpenRadioCommand => new AnotherCommandImplementation(ExecuteOpenRadio);
    public ICommand OpenStationCommand => new AnotherCommandImplementation(ExecuteOpenStation);

    private void ExecuteOpenRadio(object obj) {
      var radio = DbManager.Connection.FindRadioById(RadioActor.Radio.Id);
      // TODO 実装
    }

    private void ExecuteOpenStation(object obj) {
      var station = DbManager.Connection.FindCompanyById(RadioActor.Radio.Station.Id);
      // TODO 実装
    }

    public RadioFilmographyModel(RadioActor radioActor) {
      RadioActor = radioActor;
    }
  }
}
