using System;
using System.Collections.ObjectModel;
using System.Linq;
using SeiyuuDB.Core.Entities;
using SeiyuuDB.Core.Helpers;
using SeiyuuDB.Wpf.Utils;

namespace SeiyuuDB.Wpf.ViewModels {
  public class AddActorViewModel : Observable {
    public bool IsChanged {
      get {
        return FirstName != OldFirstName ||
          LastName != OldLastName ||
          FirstNameKana != OldFirstNameKana ||
          LastNameKana != OldLastNameKana ||
          FirstNameRomaji != OldFirstNameRomaji ||
          LastNameRomaji != OldLastNameRomaji ||
          Nickname != OldNickname ||
          Height != OldHeight ||
          Hometown != OldHometown ||
          SpouseName != OldSpouseName ||
          PictureUrl != OldPictureUrl ||
          Gender != OldGender ||
          BirthYear != OldBirthYear ||
          BirthMonth != OldBirthMonth ||
          BirthDay != OldBirthDay ||
          BloodType != OldBloodType ||
          Agency != OldAgency;
      }
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FirstNameKana { get; set; }
    public string LastNameKana { get; set; }
    public string FirstNameRomaji { get; set; }
    public string LastNameRomaji { get; set; }
    public string Nickname { get; set; }
    public int? Height { get; set; }
    public string Hometown { get; set; }
    public string SpouseName { get; set; }
    public string PictureUrl { get; set; }
    public ObservableCollection<Gender> Genders { get; private set; }
    public Gender Gender { get; set; }
    public ObservableCollection<int> BirthYears { get; private set; }
    public int? BirthYear { get; set; }
    public ObservableCollection<int> BirthMonths { get; private set; }
    public int? BirthMonth { get; set; }
    public ObservableCollection<int> BirthDays { get; private set; }
    public int? BirthDay { get; set; }
    public ObservableCollection<int> DebutYears { get; private set; }
    public int? DebutYear { get; set; }
    public ObservableCollection<BloodType> BloodTypes { get; private set; }
    public BloodType BloodType { get; set; }
    public ObservableCollection<Company> Agencies { get; private set; }
    public Company Agency { get; set; }

    public string OldFirstName { get; }
    public string OldLastName { get; }
    public string OldFirstNameKana { get; }
    public string OldLastNameKana { get; }
    public string OldFirstNameRomaji { get; }
    public string OldLastNameRomaji { get; }
    public string OldNickname { get; }
    public int? OldHeight { get; }
    public string OldHometown { get; }
    public string OldSpouseName { get;}
    public string OldPictureUrl { get; }
    public Gender OldGender { get; } = Gender.Empty;
    public int? OldBirthYear { get; }
    public int? OldBirthMonth { get; }
    public int? OldBirthDay { get; }
    public int? OldDebutYear { get; }
    public BloodType OldBloodType { get; } = BloodType.Empty;
    public Company OldAgency { get; }

    public AddActorViewModel() {
      this.BirthYears = new ObservableCollection<int>();
      this.BirthMonths = new ObservableCollection<int>();
      this.BirthDays = new ObservableCollection<int>();
      this.DebutYears = new ObservableCollection<int>();

      for (int i = DateTime.Now.Year; i >= 1930; i--) {
        this.BirthYears.Add(i);
        this.DebutYears.Add(i);
      }
      for (int i = 1; i <= 12; i++) {
        this.BirthMonths.Add(i);
      }
      for (int i = 1; i <= 31; i++) {
        this.BirthDays.Add(i);
      }

      this.Genders = new ObservableCollection<Gender>(Gender.Genders);
      this.BloodTypes = new ObservableCollection<BloodType>(BloodType.BloodTypes);
      this.Agencies = new ObservableCollection<Company>(DbManager.Connection.FindAgencies().ToList());

      this.FirstName = OldFirstName;
      this.LastName = OldLastName;
      this.FirstNameKana = OldFirstNameKana;
      this.LastNameKana = OldLastNameKana;
      this.FirstNameRomaji = OldFirstNameRomaji;
      this.LastNameRomaji = OldLastNameRomaji;
      this.Nickname = OldNickname;
      this.Height = OldHeight;
      this.Hometown = OldHometown;
      this.SpouseName = OldSpouseName;
      this.PictureUrl = OldPictureUrl;
      this.Gender = OldGender;
      this.BirthYear = OldBirthYear;
      this.BirthMonth = OldBirthMonth;
      this.BirthDay = OldBirthDay;
      this.DebutYear = OldDebutYear;
      this.BloodType = OldBloodType;
      this.Agency = OldAgency;
    }

    public AddActorViewModel(Actor actor) : this() {
      this.OldFirstName = actor.FirstName;
      this.OldLastName = actor.LastName;
      this.OldFirstNameKana = actor.FirstNameKana;
      this.OldLastNameKana = actor.LastNameKana;
      this.OldFirstNameRomaji = actor.FirstNameRomaji;
      this.OldLastNameRomaji = actor.LastNameRomaji;
      this.OldNickname = actor.Nickname;
      this.OldHeight = actor.Height;
      this.OldHometown = actor.Hometown;
      this.OldSpouseName = actor.SpouseName;
      this.OldPictureUrl = actor.PictureUrl;
      this.OldGender = actor.Gender;
      this.OldBirthYear = actor.Birthdate.Year;
      this.OldBirthMonth = actor.Birthdate.Month;
      this.OldBirthDay = actor.Birthdate.Day;
      this.OldDebutYear = actor.DebutYear;
      this.OldBloodType = actor.BloodType;
      this.OldAgency = actor.Agency;
    }
  }
}
