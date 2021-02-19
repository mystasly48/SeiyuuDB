using SeiyuuDB.Core.Entities;
using SeiyuuDB.Wpf.Utils;
using System.Windows.Media;

namespace SeiyuuDB.Wpf.Models {
  public class ActorCardModel {
    public int ActorId { get; set; }
    public string ActorLastName { get; set; }
    public string ActorFirstName { get; set; }
    public string ActorLastNameKana { get; set; }
    public string ActorFirstNameKana { get; set; }
    public string AgencyName { get; set; }
    public string Birthdate { get; set; }
    private string _pictureUrl;
    public ImageSource Picture => string.IsNullOrEmpty(_pictureUrl) ? ImageHelper.NoImage : ImageHelper.UrlToBitmapImage(_pictureUrl);

    public ActorCardModel(Actor actor) {
      ActorId = actor.Id;
      ActorLastName = actor.LastName;
      ActorFirstName = actor.FirstName;
      ActorLastNameKana = actor.LastNameKana;
      ActorFirstNameKana = actor.FirstNameKana;
      AgencyName = actor.Agency?.Name;
      Birthdate = actor.Birthdate.ToString();
      _pictureUrl = actor.PictureUrl;
    }
  }
}
