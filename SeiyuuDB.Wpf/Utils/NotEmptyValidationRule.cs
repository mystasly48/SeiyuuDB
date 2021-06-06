﻿using System.Globalization;
using System.Windows.Controls;

namespace SeiyuuDB.Wpf.Utils {
  public class NotEmptyValidationRule : ValidationRule {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
      return string.IsNullOrWhiteSpace((value ?? "").ToString())
          ? new ValidationResult(false, "入力必須")
          : ValidationResult.ValidResult;
    }
  }
}
