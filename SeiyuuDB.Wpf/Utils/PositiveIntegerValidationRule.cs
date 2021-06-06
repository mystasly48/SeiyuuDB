using System.Globalization;
using System.Windows.Controls;

namespace SeiyuuDB.Wpf.Utils {
  public class PositiveIntegerValidationRule : ValidationRule {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
      bool parseResult = int.TryParse((value ?? "").ToString(), out int res);
      if (parseResult && 0 <= res) {
        return ValidationResult.ValidResult;
      } else if (!parseResult) {
        return new ValidationResult(false, "数値を入力");
      } else {
        return new ValidationResult(false, "非負整数を入力");
      }
    }
  }
}
