using SeiyuuDB.Core.Entities;
using SeiyuuDB.Wpf.Utils;
using System.Windows.Media;

namespace SeiyuuDB.Wpf.Models {
  public class ActorCardModel {
    private Actor _actor;

    public int ActorId => _actor.Id;
    public string LastName => _actor.LastName;
    public string FirstName => _actor.FirstName;
    public string LastNameKana => _actor.LastNameKana;
    public string FirstNameKana => _actor.FirstNameKana;
    public string AgencyName => _actor.Agency?.Name;
    public string Birthdate => _actor.Birthdate.ToString();
    public ImageSource Picture => string.IsNullOrEmpty(_actor.PictureUrl)
      ? ImageHelper.NoImage
      : ImageHelper.UrlToBitmapImage(_actor.PictureUrl);

    public ActorCardModel(Actor actor) {
      _actor = actor;
    }
  }
}
